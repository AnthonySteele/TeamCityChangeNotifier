using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCityChangeNotifier.Http;
using TeamCityChangeNotifier.Parsers;

namespace TeamCityChangeNotifier
{
	public class TeamCityOperations
	{
		private readonly TeamCityReader _reader = new TeamCityReader();

		public async Task<ChangeSet> ChangesForRelease(int buildId)
		{
			var releaseBuild = await GetBuild(buildId);
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
				var changeResponse = await _reader.ReadRelativeUrl(changeHref);

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
				var buildChangesData = await _reader.ReadBuildChanges(buildId);
				var changesInThisBuild = new ChangeListXmlParser(buildChangesData);

				hrefs.AddRange(changesInThisBuild.ChangeHrefs());
			}

			return hrefs;
		}

		private async Task<List<int>>  BuildsIdsBackToLastPin(int firstBuildId, string firstBuildType)
		{
			var buildListData = await _reader.ReadBuildList(firstBuildType);
			var buildList = new BuildListXmlParser(buildListData);

			return buildList.FromIdBackToLastPin(firstBuildId);
		}

		private async Task<BuildXmlParser> GetBuild(int buildId)
		{
			var releaseData = await _reader.ReadBuild(buildId);
			return new BuildXmlParser(releaseData);
		}
	}
}
