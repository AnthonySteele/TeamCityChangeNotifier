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
			var projectName = releaseBuild.ProjectName();

			var firstBuildId = releaseBuild.FirstBuildId();
			var firstBuild = await GetBuild(firstBuildId);
			var firstBuildType = firstBuild.BuildType();

			var buildListData= await BuildsIdsBackToLastPin(firstBuildId, firstBuildType);
			var changeHrefs = await ReadAllChangeHrefsFromBuilds(buildListData.Ids);
			var changes = await ReadAllChanges(changeHrefs);

			return new ChangeSet
				{
					ProjectName = projectName,
					PinnedBuildId = firstBuildId,
					Builds = buildListData,
					Changes = changes
				};
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

		private async Task<BuildListData> BuildsIdsBackToLastPin(int firstBuildId, string firstBuildType)
		{
			var buildListData = await reader.ReadBuildList(firstBuildType);
			var buildList = new BuildListXmlParser(buildListData);

			return buildList.FromIdBackToLastPin(firstBuildId);
		}

		private async Task<BuildXmlParser> GetBuild(int buildId)
		{
			var releaseData = await reader.ReadBuild(buildId);
			return new BuildXmlParser(releaseData);
		}
	}
}
