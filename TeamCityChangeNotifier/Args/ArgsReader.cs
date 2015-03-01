namespace TeamCityChangeNotifier.Args
{
	public class ArgsReader
	{
		public Request ReadArgs(string[] args)
		{
			if (args.Length == 0)
			{
				return Request.Error("Please supply a teamcity build number");
			}

			int buildId;
			var parsed = int.TryParse(args[0], out buildId);
			if (!parsed)
			{
				var message = string.Format("The argument '{0}' is not a teamcity build number", args[0]);
				return Request.Error(message);
			}

			var sourceBuildTitleText = "Build";
			if (args.Length >= 2)
			{
				sourceBuildTitleText = args[1];
			}

			return new Request
				{
					ArgsError = false,
					InitialBuildId = buildId,
					SourceBuildTitleText = sourceBuildTitleText
				};
		}
	}
}
