using System.Threading.Tasks;

namespace TeamCityChangeNotifier.Http
{
	public interface ITeamCityReader
	{
		Task<string> ReadBuildList(string buildName);
		Task<string> ReadBuild(int buildId);
		Task<string> ReadBuildChanges(int buildId);
		Task<string> ReadChange(int changeId);
		Task<string> ReadRelativeUrl(string relativeUrl);
	}
}