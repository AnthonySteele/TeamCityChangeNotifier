using System;
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

			TeamCityUrl = Read("TeamcityUrl", true);
			TeamCityRestUrl = UriPath.Combine(TeamCityUrl, "httpAuth/app/rest");

			SmtpHost = Read("SmtpHost", true);
			DestinationEmail = Read("DestinationEmail", true);
			SenderEmail = Read("SenderEmail", true);
		}

		private string Read(string key, bool mandatory = false)
		{
			var result = ConfigurationManager.AppSettings[key];
			if (mandatory && string.IsNullOrEmpty(result))
			{
				throw new Exception("Did not find config value for " + key);
			}
			return result;
		}
	}
}
