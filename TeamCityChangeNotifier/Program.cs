using System;
using System.Threading.Tasks;

using TeamCityChangeNotifier.Args;
using TeamCityChangeNotifier.Email;

namespace TeamCityChangeNotifier
{
	class Program
	{
		static void Main(string[] args)
		{
			var argsReader = new ArgsReader();
			var request = argsReader.ReadArgs(args);

			if (request.ArgsError)
			{
				Console.WriteLine(request.ArgsErrorMessage);
				return;
			}


			var task = TeamCityChangesForRelease(request);
			task.Wait();

			//Console.ReadLine();
		}

		private static async Task TeamCityChangesForRelease(Request request)
		{
			try
			{
				var reader = new TeamCityOperations();
				var changes = await reader.ChangesForRelease(request);

				var sender = new EmailSender();
				sender.SendNotification(changes);

				Console.WriteLine(changes.Summary());
				Console.WriteLine("success");

			}
			catch (Exception ex)
			{
				Console.WriteLine("failed");
				Console.WriteLine(ex.Message);
			}
		}
	}
}
