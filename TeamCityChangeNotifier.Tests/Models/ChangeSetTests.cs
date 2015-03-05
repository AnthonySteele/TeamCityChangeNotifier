using System;
using System.Collections.Generic;
using NUnit.Framework;
using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.Tests.Models
{
	[TestFixture]
	public class ChangeSetTests
	{
		[Test]
		public void HasDefaultSummary()
		{
			var changeSet = MakeEmptyChangeSet();
			Assert.That(changeSet.Summary(), Is.EqualTo("Release to Bar with no changes in no builds over 0 hours"));
		}

		[Test]
		public void HasDefaultDetails()
		{
			var changeSet = MakeEmptyChangeSet();
			Assert.That(changeSet.Details(), Is.EqualTo("Release to Bar with no changes in no builds over 0 hours"));
		}
		
		private ChangeSet MakeEmptyChangeSet()
		{
			return new ChangeSet
			{
				Builds = new BuildListData
				{
					Ids = new List<int>(),
					PreviousPinned = MakeBuildData(),
					LatestBuild = MakeBuildData()
				},
				Changes = new List<ChangeData>(),
				ReleaseBuild = MakeBuildData()
			};
		}

		private BuildData MakeBuildData()
		{
			var start = DateTime.Now.AddHours(-1);

			return new BuildData
				{
					Id = 123,
					BuildType = "foo",
					DependencyBuildId = 1234,
					StartDate = start,
					FinishDate = start.AddMinutes(1),
					ProjectName = "Bar"
				};
		}
	}
}
