using System.Collections.Generic;
using System.Threading.Tasks;

using TeamCityChangeNotifier.Args;
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

			var buildIds = await BuildsIdsBackToLastPin(firstBuildId, firstBuildType);
			var changeHrefs = await ReadAllChangeHrefsFromBuilds(buildIds);
			var changes = await ReadAllChanges(changeHrefs);

			return new ChangeSet
				{
					ProjectName = projectName,
					PinnedBuildId = firstBuildId,
					AllBuilds = buildIds,
					Changes = changes
				};
		}

		private async Task<List<ChangeData>> ReadAllChanges(List<string> changeHrefs)
		{
			var changes = new List<ChangeData>();
			var parser = new ChangeDataParser();

			foreach (var changeHref in changeHrefs)
			{
				var changeResponse = await reader.ReadRelativeUrl(changeHref);

				var change = parser.ReadXml(changeResponse);
				changes.Add(change);
			}

			return changes;
		}

		private async Task<List<string>> ReadAllChangeHrefsFromBuilds(List<int> buildIds)
		{
			var hrefs = new List<string>();

			foreach (var buildId in buildIds)
			{
				var buildChangesData = await reader.ReadBuildChanges(buildId);
				var changesInThisBuild = new ChangeListXmlParser(buildChangesData);

				hrefs.AddRange(changesInThisBuild.ChangeHrefs());
			}

			return hrefs;
		}

		private async Task<List<int>>  BuildsIdsBackToLastPin(int firstBuildId, string firstBuildType)
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
