using System.Threading.Tasks;

using TeamCityChangeNotifier.Http;

namespace TeamCityChangeNotifier.Tests
{
	public class FakeTeamCityReader : ITeamCityReader
	{
		private string _value;

		public FakeTeamCityReader(string value)
		{
			_value = value;
		}

		public Task<string> ReadBuildList(string buildName)
		{
			return Task.FromResult(_value);
		}

		public Task<string> ReadBuild(int buildId)
		{
			return Task.FromResult(_value);
		}

		public Task<string> ReadBuildChanges(int buildId)
		{
			return Task.FromResult(_value);
		}

		public Task<string> ReadChange(int changeId)
		{
			return Task.FromResult(_value);
		}

		public Task<string> ReadRelativeUrl(string relativeUrl)
		{
			return Task.FromResult(_value);
		}
	}
}