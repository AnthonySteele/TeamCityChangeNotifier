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

		[Test]
		public void HasOneChangeSummary()
		{
			var changeSet = MakeChangeSet();
			Assert.That(changeSet.Summary(), Is.EqualTo("Release to Bar with 1 change in 1 build over 2 hours"));
		}

		[Test]
		public void HasOneChangeDetails()
		{
			const string expected = 
@"Release to Bar with 1 change in 1 build over 2 hours

anthony on Tue 05/05/2015 00:00
this is a change";

			var changeSet = MakeChangeSet();
			Assert.That(changeSet.Details(), Is.EqualTo(expected));
		}

		private ChangeSet MakeChangeSet()
		{
			var result = MakeEmptyChangeSet();
			result.Builds.Ids.Add(12345);
			result.Builds.PreviousPinned.FinishDate = result.Builds.LatestBuild.FinishDate.AddHours(-2);

			result.Changes.Add(MakeChangeData());
			return result;
		}

		private ChangeData MakeChangeData()
		{
			return new ChangeData
			{
				Id = 123,
				Author = "anthony",
				Message = "this is a change",
				Date = new DateTime(2015, 5, 5)
			};
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
