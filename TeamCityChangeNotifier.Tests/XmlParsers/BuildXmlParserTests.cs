using System;
using NUnit.Framework;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Tests.XmlParsers
{
	[TestFixture]
	public class BuildXmlParserTests
	{
		private const string BuildXmlWithoutEnding =
			@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
	<build id=""12345"" buildTypeId=""my_build_type"" number=""1.0.1234"" status=""SUCCESS"" state=""finished"" href=""/httpAuth/app/rest/builds/id:12345"" webUrl=""http://teamcity.win.sys.7d/viewLog.html?buildId=12345&amp;my_build_type"">
	<statusText>Tests passed: 106</statusText>
	<buildType id=""my_build_type"" name=""Build, Unit Test"" projectName=""myProject"" projectId=""myprojectId"" href=""/httpAuth/app/rest/buildTypes/id:myProject"" webUrl=""http://teamcity.win.sys.7d/viewType.html?buildTypeId=myProject""/>
	<tags/>
	<queuedDate>20150303T070007+0000</queuedDate><startDate>20150303T070008+0000</startDate><finishDate>20150303T070221+0000</finishDate>
	<triggered type=""unknown"" details=""Schedule Trigger"" date=""20150303T070007+0000""/>
	<lastChanges count=""1"">
	<change id=""21132"" version=""e8c186af1eb1e13920974e0fdb4de14c9ecfd75a"" username=""dan"" date=""20150302T110057+0000"" href=""/httpAuth/app/rest/changes/id:2345"" webLink=""http://teamcity.win.sys.7d/viewModification.html?modId=2345&amp;personal=false""/></lastChanges>
	<changes href=""/httpAuth/app/rest/changes?locator=build:(id:12345)""/>
	<revisions><revision version=""e8c186af1eb1e13920974e0fdb4de14c9ecfd75a""><vcs-root-instance id=""484"" vcs-root-id=""myProject"" name=""merchandising-api"" href=""/httpAuth/app/rest/vcs-root-instances/id:484""/></revision></revisions>
	<agent id=""3"" name=""buildagent-a"" typeId=""3"" href=""/httpAuth/app/rest/agents/id:3""/><testOccurrences count=""106"" href=""/httpAuth/app/rest/testOccurrences?locator=build:(id:12345)"" passed=""106""/>
	<artifacts href=""/httpAuth/app/rest/builds/id:12345/artifacts/children""/>";

	private const string BuildXmlWithoutDependency = BuildXmlWithoutEnding + "</build>";

	private const string BuildXmlWithDependency = BuildXmlWithoutEnding +
		@"<artifact-dependencies count=""1"">
			<build id=""34567"" buildTypeId=""my_build_type"" number=""1.0.466"" status=""SUCCESS"" state=""finished"" pinned=""true"" href=""/httpAuth/app/rest/builds/id:12345"" webUrl=""http://teamcity.win.sys.7d/viewLog.html?buildId=12345&amp;buildTypeId=my_Build_type""/>
		</artifact-dependencies>
		</build>";

		[Test]
		public void CanParse()
		{
			var parser = new BuildXmlParser(BuildXmlWithoutDependency);
			var buildData = parser.GetBuildData();

			Assert.That(buildData, Is.Not.Null);
			Assert.That(buildData.Id, Is.EqualTo(12345));
			Assert.That(buildData.BuildType, Is.EqualTo("my_build_type"));
			Assert.That(buildData.StartDate, Is.EqualTo(new DateTime(2015, 3, 3, 7, 0, 8)));
			Assert.That(buildData.FinishDate, Is.EqualTo(new DateTime(2015, 3, 3, 7, 2, 21)));
			Assert.That(buildData.DependencyBuildId, Is.Null);
		}

		[Test]
		public void CanParseWithDependency()
		{
			var parser = new BuildXmlParser(BuildXmlWithDependency);
			var buildData = parser.GetBuildData();

			Assert.That(buildData, Is.Not.Null);
			Assert.That(buildData.DependencyBuildId, Is.EqualTo(34567));
		}
	}
}
