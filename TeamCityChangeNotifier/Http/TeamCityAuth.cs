using System;
using System.Text;
using TeamCityChangeNotifier.Args;
using TeamCityChangeNotifier.Helpers;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Http
{
	public class TeamCityAuth
	{
		private readonly ConfigSettings settings = new ConfigSettings();

		public static Request Request { get; set; }

		public string AuthInfo()
		{
			var teamCityUser = ReadTeamCityUser();

			if (string.IsNullOrEmpty(teamCityUser))
			{
				throw new ParseException("No teamcity user name found in config or commandline");
			}

			var teamCityPassword = ReadTeamCityPassword();
			if (string.IsNullOrEmpty(teamCityPassword))
			{
				throw new ParseException("No teamcity password found in config or commandline");
			}

			string authInfo = teamCityUser + ":" + teamCityPassword;
			var encodedAuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			return encodedAuthInfo;
		}

		private string ReadTeamCityUser()
		{
			if (!string.IsNullOrEmpty(settings.TeamCityUser))
			{
				return settings.TeamCityUser;
			}

			if (Request != null)
			{
				return Request.TeamCityUser;
			}

			return null;
		}

		private string ReadTeamCityPassword()
		{
			if (!string.IsNullOrEmpty(settings.TeamCityPassword))
			{
				return settings.TeamCityPassword;
			}

			if (Request != null)
			{
				return Request.TeamCityPassword;
			}

			return null;
		}
	}
}
