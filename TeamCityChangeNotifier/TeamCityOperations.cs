using System;
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
		private readonly TeamCityReader _reader;
		private readonly Request _request;

		public TeamCityOperations(TeamCityReader reader, Request request)
		{
			_request = request;
			_reader = reader; 
		}

		public async Task<ChangeSet> ChangesForRelease()
		{
			var releaseBuildData = await ReadBuild(_request.InitialBuildId);
			var sourceBuildData = await FindSourceBuild(releaseBuildData);
			var buildListData = await BuildsIdsBackToLastPin(sourceBuildData);
			var changeHrefs = await ReadAllChangeHrefsFromBuilds(buildListData.Ids);
			var changes = await ReadAllChanges(changeHrefs);

			var changeSet = new ChangeSet
				{
					ReleaseBuild = releaseBuildData,
					Builds = buildListData,
					Changes = changes
				};

			Console.WriteLine("Notifying on " + changeSet.Summary());

			return changeSet;
		}

		public async Task<BuildData> FindSourceBuild(BuildData releaseBuildData)
		{
			var sourceBuildId = releaseBuildData.DependencyBuildId;
			if (!sourceBuildId.HasValue)
			{
				throw new ParseException("No source build id found");
			}

			var sourceBuildData = await ReadBuild(sourceBuildId.Value);
			return sourceBuildData;
		}

		private async Task<List<ChangeData>> ReadAllChanges(List<string> changeHrefs)
		{
			var changeResponses = await Tasks.ReadParallel(changeHrefs, url => _reader.ReadRelativeUrl(url));

			var parser = new ChangeDataParser();
			return changeResponses
				.Select(changeResponse => parser.ReadXml(changeResponse))
				.ToList();
		}

		private async Task<List<string>> ReadAllChangeHrefsFromBuilds(List<int> buildIds)
		{
			var buildChangesDataList = await Tasks.ReadParallel(buildIds, id => _reader.ReadBuildChanges(id));

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
			var buildListData = await _reader.ReadBuildList(latestBuild.BuildType);
			var buildList = new BuildListXmlParser(buildListData);

			var result =  buildList.FromIdBackToLastPin(latestBuild.Id);
			result.LatestBuild = latestBuild;

			if (result.PreviousPinnedBuildId > 0)
			{
				var prevPinnedData = await ReadBuild(result.PreviousPinnedBuildId);
				result.PreviousPinned = prevPinnedData;
			}

			return result;
		}

		private async Task<BuildData> ReadBuild(int buildId)
		{
			var releaseData = await _reader.ReadBuild(buildId);
			var parser = new BuildXmlParser(releaseData);
			var data = parser.GetBuildData();

			var message = string.Format("Read build {0} in {1} - {2}", 
				data.Id, data.ProjectName, data.BuildType);
			Console.WriteLine(message);

			return data;
		}
	}
}
