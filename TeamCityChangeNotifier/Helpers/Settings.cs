using System.Configuration;

namespace TeamCityChangeNotifier.Helpers
{
	public class Settings
	{
		public string TeamCityAuthInfo { get; private set; }
		public string TeamCityUser { get; private set; }
		public string TeamCityPassword { get; private set; }

		public string TeamCityUrl { get; private set; }
		public string TeamCityRestUrl { get; private set; }

		public string SmtpHost { get; private set; }
		public string DestinationEmail { get; private set; }
		public int TestBuildId { get; set; }

		public Settings()
		{
			TeamCityAuthInfo = Read("TeamcityAuthInfo");
			TeamCityUser = Read("TeamCityUser");
			TeamCityPassword = Read("TeamCityPassword");

			TeamCityUrl = Read("TeamcityUrl");
			TeamCityRestUrl = UriPath.Combine(TeamCityUrl, "httpAuth/app/rest");

			SmtpHost = Read("SmtpHost");
			DestinationEmail = Read("DestinationEmail");
		}

		private string Read(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
