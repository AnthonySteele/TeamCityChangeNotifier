using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Http
{
	public class TeamCityReader
	{
		private readonly Settings _settings = new Settings();
		private readonly HttpReader _reader = new HttpReader();

		public async Task<string> ReadBuildList(string buildName)
		{
			var id = "id:" + buildName;
			var url = UriPath.Combine(_settings.TeamcityRestUrl, "buildTypes", id, "builds");
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadBuild(int buildId)
		{
			var url = UriPath.Combine(_settings.TeamcityRestUrl, "builds", buildId.ToString());
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadBuildChanges(int buildId)
		{
			var id = string.Format("?locator=build:(id:{0})",buildId);
			var url = UriPath.Combine(_settings.TeamcityRestUrl, "changes", id);
			return await _reader.ReadResponseBody(url);
		}


		public async Task<string> ReadChange(int changeId)
		{
			var id = "id:" + changeId;
			var url = UriPath.Combine(_settings.TeamcityRestUrl, "changes", id);
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadRelativeUrl(string relativeUrl)
		{
			var url = UriPath.Combine(_settings.TeamcityUrl, relativeUrl);
			return await _reader.ReadResponseBody(url);
		}
	}
}
