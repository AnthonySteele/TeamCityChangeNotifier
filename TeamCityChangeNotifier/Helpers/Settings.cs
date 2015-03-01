using System.Configuration;
using System.Runtime.CompilerServices;

namespace TeamCityChangeNotifier.Helpers
{
	public class Settings
	{
		public string TeamcityAuthInfo { get; private set; }
		public string TeamcityUrl { get; private set; }
		public string TeamcityRestUrl { get; private set; }

		public string SmtpHost { get; private set; }
		public string DestinationEmail { get; private set; }
		public int TestBuildId
		{ get; set; }

		public Settings()
		{
			TeamcityAuthInfo = Read("TeamcityAuthInfo");
			TeamcityUrl = Read("TeamcityUrl");
			TeamcityRestUrl = UriPath.Combine(TeamcityUrl, "httpAuth/app/rest");

			SmtpHost = Read("SmtpHost");
			DestinationEmail = ("DestinationEmail");
		}

		private string Read(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
