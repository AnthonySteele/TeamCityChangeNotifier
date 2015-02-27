using System;
using System.Threading.Tasks;
using TeamCityChangeNotifier.Email;

namespace TeamCityChangeNotifier
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Please supply a teamcity build number");
				return;
			}

			int buildId;
			var parsed = int.TryParse(args[0], out buildId);
			if (!parsed)
			{
				var message = string.Format("The argument '{0}' is not a teamcity build number", args[0]);
				Console.WriteLine(message);
				return;
			}

			var task = TeamCityChangesForRelease(buildId);
			task.Wait();
		}

		private static async Task TeamCityChangesForRelease(int buildId)
		{
			try
			{
				var reader = new TeamCityOperations();
				var changes = await reader.ChangesForRelease(buildId);
				//Console.WriteLine(changes.Details());

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
