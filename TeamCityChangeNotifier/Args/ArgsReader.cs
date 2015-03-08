namespace TeamCityChangeNotifier.Args
{
	public class ArgsReader
	{
		public Request ReadArgs(string[] args)
		{
			int buildId = 0;
			string teamcityUser = null;
			string teamcityPassword = null;
			
			foreach (var arg in args)
			{
				if (arg.StartsWith("id:"))
				{
					var idArg = arg.Substring(3);
					var parsed = int.TryParse(idArg, out buildId);
					if (!parsed)
					{
						var message = string.Format("The argument '{0}' is not a teamcity build number", args[0]);
						return Request.Error(message);
					}
				}
				else if (arg.StartsWith("pw:"))
				{
					teamcityPassword = arg.Substring(3);
				}
				else if (arg.StartsWith("u:"))
				{
					teamcityUser = arg.Substring(2);
				}
				else
				{
					return Request.Error("Unknown argument " + arg);
				}
			}

			if (buildId == 0)
			{
				return Request.Error("Please supply a teamcity build number");
			}

			return new Request
				{
					ArgsError = false,
					InitialBuildId = buildId,
					TeamCityUser = teamcityUser,
					TeamCityPassword = teamcityPassword
				};
		}
	}
}
