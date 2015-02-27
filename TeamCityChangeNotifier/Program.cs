using System;
using System.Threading.Tasks;
using TeamCityChangeNotifier.Email;

namespace TeamCityChangeNotifier
{
	class Program
	{
		static void Main(string[] args)
		{
			int buildId;
			if (args.Length > 0)
			{
				buildId = int.Parse(args[0]);
			}
			else
			{
				buildId = 209535;
			}

			var task = TeamCityChangesForRelease(buildId);
			task.Wait();

			Console.ReadLine();
		}

		private static async Task TeamCityChangesForRelease(int buildId)
		{
			try
			{
				var reader = new TeamCityOperations();
				var changes = await reader.ChangesForRelease(buildId);
				Console.WriteLine(changes.Details());

				var sender = new EmailSender();
				sender.SendNotification(changes);

			}
			catch (Exception ex)
			{
				Console.WriteLine("failed");
				Console.WriteLine(ex.Message);
			}
		}
	}
}
