using System.Configuration;

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
			TeamcityAuthInfo = ConfigurationManager.AppSettings["TeamcityAuthInfo"];
			TeamcityUrl = ConfigurationManager.AppSettings["TeamcityUrl"];
			TeamcityRestUrl = UriPath.Combine(TeamcityUrl, "httpAuth/app/rest");

			SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
			DestinationEmail = ConfigurationManager.AppSettings["DestinationEmail"];
		}
	}
}
