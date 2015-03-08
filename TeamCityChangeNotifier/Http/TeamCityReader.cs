using System.Threading.Tasks;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Http
{
	public class TeamCityReader : ITeamCityReader
	{
		private readonly ConfigSettings _settings = new ConfigSettings();
		private readonly HttpReader _reader;

		public TeamCityReader(HttpReader reader)
		{
			_reader = reader;
		}

		public async Task<string> ReadBuildList(string buildName)
		{
			var id = "id:" + buildName;
			var url = UriPath.Combine(_settings.TeamCityRestUrl, "buildTypes", id, "builds");
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadBuild(int buildId)
		{
			var url = UriPath.Combine(_settings.TeamCityRestUrl, "builds", buildId.ToString());
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadBuildChanges(int buildId)
		{
			var id = string.Format("?locator=build:(id:{0})",buildId);
			var url = UriPath.Combine(_settings.TeamCityRestUrl, "changes", id);
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadChange(int changeId)
		{
			var id = "id:" + changeId;
			var url = UriPath.Combine(_settings.TeamCityRestUrl, "changes", id);
			return await _reader.ReadResponseBody(url);
		}

		public async Task<string> ReadRelativeUrl(string relativeUrl)
		{
			var url = UriPath.Combine(_settings.TeamCityUrl, relativeUrl);
			return await _reader.ReadResponseBody(url);
		}
	}
}
