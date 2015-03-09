using System;
using System.Text;
using TeamCityChangeNotifier.Args;
using TeamCityChangeNotifier.Helpers;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Http
{
	public class TeamCityAuth
	{
		private readonly ConfigSettings _settings = new ConfigSettings();
		private readonly Request _request;

		public TeamCityAuth(Request request)
		{
			_request = request;
		}

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
			if (_request != null && (! string.IsNullOrEmpty(_request.TeamCityUser)))
			{
				return _request.TeamCityUser;
			}

			if (!string.IsNullOrEmpty(_settings.TeamCityUser))
			{
				return _settings.TeamCityUser;
			}

			return null;
		}

		private string ReadTeamCityPassword()
		{
			if (_request != null && (!string.IsNullOrEmpty(_request.TeamCityPassword)))
			{
				return _request.TeamCityPassword;
			}

			if (!string.IsNullOrEmpty(_settings.TeamCityPassword))
			{
				return _settings.TeamCityPassword;
			}

			return null;
		}
	}
}
