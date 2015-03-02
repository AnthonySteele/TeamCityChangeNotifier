using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TeamCityChangeNotifier.Args;
using TeamCityChangeNotifier.Helpers;
using TeamCityChangeNotifier.Http;
using TeamCityChangeNotifier.Models;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier
{
	public class TeamCityOperations
	{
		private readonly TeamCityReader reader = new TeamCityReader();

		public async Task<ChangeSet> ChangesForRelease(Request request)
		{
			var releaseBuild = await GetBuild(request.InitialBuildId);
			var releaseBuildData = releaseBuild.GetBuildData();

			var sourceBuildData = await FindSourceBuild(releaseBuildData);

			var buildListData = await BuildsIdsBackToLastPin(sourceBuildData);

			var changeHrefs = await ReadAllChangeHrefsFromBuilds(buildListData.Ids);
			var changes = await ReadAllChanges(changeHrefs);

			return new ChangeSet
				{
					ReleaseBuild = releaseBuildData,
					PinnedBuildId = sourceBuildData.Id,
					Builds = buildListData,
					Changes = changes
				};
		}

		public async Task<BuildData> FindSourceBuild(BuildData releaseBuildData)
		{
			var sourceBuildId = releaseBuildData.DependencyBuildId;
			if (!sourceBuildId.HasValue)
			{
				throw new ParseException("No source build id found");
			}

			var sourceBuild = await GetBuild(sourceBuildId.Value);
			var sourceBuildData = sourceBuild.GetBuildData();
			sourceBuildData.Id = sourceBuildId.Value; // should not be needed
			return sourceBuildData;
		}

		private async Task<List<ChangeData>> ReadAllChanges(List<string> changeHrefs)
		{
			var changeResponses = await Tasks.ReadParallel(changeHrefs, url => reader.ReadRelativeUrl(url));

			var parser = new ChangeDataParser();
			return changeResponses
				.Select(changeResponse => parser.ReadXml(changeResponse))
				.ToList();
		}

		private async Task<List<string>> ReadAllChangeHrefsFromBuilds(List<int> buildIds)
		{
			var buildChangesDataList = await Tasks.ReadParallel(buildIds, id => reader.ReadBuildChanges(id));

			var hrefs = new List<string>();
			foreach (var buildChangesData in buildChangesDataList)
			{
				var changesInThisBuild = new ChangeListXmlParser(buildChangesData);
				hrefs.AddRange(changesInThisBuild.ChangeHrefs());
			}

			return hrefs;
		}

		private async Task<BuildListData> BuildsIdsBackToLastPin(BuildData latestBuild)
		{
			var buildListData = await reader.ReadBuildList(latestBuild.BuildType);
			var buildList = new BuildListXmlParser(buildListData);

			var result =  buildList.FromIdBackToLastPin(latestBuild.Id);
			result.LatestBuild = latestBuild;

			if (result.PreviousPinnedBuildId > 0)
			{
				var prevPinnedData = await GetBuild(result.PreviousPinnedBuildId);
				result.PreviousPinned = prevPinnedData.GetBuildData();
			}

			return result;
		}

		private async Task<BuildXmlParser> GetBuild(int buildId)
		{
			var releaseData = await reader.ReadBuild(buildId);
			return new BuildXmlParser(releaseData);
		}
	}
}
