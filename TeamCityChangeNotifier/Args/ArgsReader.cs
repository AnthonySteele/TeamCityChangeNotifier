using System.Collections.Generic;

namespace TeamCityChangeNotifier.Args
{
	public class ArgsReader
	{
		public Request ReadArgs(IEnumerable<string> args)
		{
			int? buildId = null;
			string teamcityUser = null;
			string teamcityPassword = null;
			
			foreach (var arg in args)
			{
				if (MatchesPrefix(arg,"pw"))
				{
					teamcityPassword = AfterPrefix(arg, "pw");
				}
				else if (MatchesPrefix(arg, "u"))
				{
					teamcityUser = AfterPrefix(arg, "u");
				}
				else if (MatchesPrefix(arg, "id"))
				{
					var idArg = AfterPrefix(arg, "id");
					int value;
					var parsed = int.TryParse(idArg, out value);
					if (parsed)
					{
						buildId = value;
					}
					else
					{
						var message = string.Format("The argument '{0}' is not a number", arg);
						return Request.Error(message);
					}
				}
				else if (IsInt(arg))
				{
					buildId = int.Parse(arg);
				}
				else
				{
					return Request.Error("Unknown argument " + arg);
				}
			}

			if (! buildId.HasValue)
			{
				return Request.Error("Please supply a teamcity build number");
			}

			return new Request
				{
					ArgsError = false,
					InitialBuildId = buildId.Value,
					TeamCityUser = teamcityUser,
					TeamCityPassword = teamcityPassword
				};
		}

		private static bool IsInt(string valueIn)
		{
			int valueOut;
			var parsed = int.TryParse(valueIn, out valueOut);
			return parsed;
		}

		private static bool MatchesPrefix(string arg, string prefix)
		{
			return arg.StartsWith(prefix + ":");
		}

		private static string AfterPrefix(string arg, string prefix)
		{
			return arg.Substring(prefix.Length + 1);
		}
	}
}
