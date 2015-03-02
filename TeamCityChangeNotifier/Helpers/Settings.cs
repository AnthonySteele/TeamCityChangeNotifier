using System;
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
			TeamCityUser = ReadEnvOrConfig("TeamCityUser");
			TeamCityPassword = ReadEnvOrConfig("TeamCityPassword");

			TeamCityUrl = Read("TeamcityUrl");
			TeamCityRestUrl = UriPath.Combine(TeamCityUrl, "httpAuth/app/rest");

			SmtpHost = Read("SmtpHost");
			DestinationEmail = Read("DestinationEmail");
		}

		private string Read(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		private static string ReadEnvOrConfig(string key)
		{
			var valueFromEnv = Environment.GetEnvironmentVariable(key);
			return valueFromEnv ?? ConfigurationManager.AppSettings[key];
		}
	}
}
