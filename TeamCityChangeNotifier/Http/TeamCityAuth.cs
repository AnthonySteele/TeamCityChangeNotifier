using System;
using System.Text;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Http
{
	public class TeamCityAuth
	{
		private readonly Settings settings = new Settings();

		public string AuthInfo()
		{
			if (!string.IsNullOrEmpty(settings.TeamCityAuthInfo))
			{
				return settings.TeamCityAuthInfo;
			}

			string authInfo = settings.TeamCityUser + ":" + settings.TeamCityPassword;
			var encodedAuthInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			return encodedAuthInfo;
		}
	}
}
