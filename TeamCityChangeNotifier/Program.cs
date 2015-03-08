using System;
using System.Threading.Tasks;

using TeamCityChangeNotifier.Args;
using TeamCityChangeNotifier.Email;
using TeamCityChangeNotifier.Http;

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
				var teamCityOperations = BuildTeamCityOperations(request);

				var changes = await teamCityOperations.ChangesForRelease();

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

		private static TeamCityOperations BuildTeamCityOperations(Request request)
		{
			var teamCityReader = new TeamCityReader(new HttpReader(new TeamCityAuth(request)));
			return new TeamCityOperations(teamCityReader, request);
		}
	}
}
