using System.Configuration;

namespace TeamCityChangeNotifier.Helpers
{
	public class ConfigSettings
	{
		public string TeamCityUser { get; private set; }
		public string TeamCityPassword { get; private set; }

		public string TeamCityUrl { get; private set; }
		public string TeamCityRestUrl { get; private set; }

		public string SmtpHost { get; private set; }
		public string DestinationEmail { get; private set; }
		public string SenderEmail { get; private set; }

		public ConfigSettings()
		{
			TeamCityUser = Read("TeamCityUser");
			TeamCityPassword = Read("TeamCityPassword");

			TeamCityUrl = Read("TeamcityUrl");
			TeamCityRestUrl = UriPath.Combine(TeamCityUrl, "httpAuth/app/rest");

			SmtpHost = Read("SmtpHost");
			DestinationEmail = Read("DestinationEmail");
			SenderEmail = Read("SenderEmail");
		}

		private string Read(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
