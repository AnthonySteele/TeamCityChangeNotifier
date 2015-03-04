using NUnit.Framework;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Tests.XmlParsers
{
	[TestFixture]
	public class BuildListXmlParserTests
	{
		private const string BuildListXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<builds count=""49"" href=""/httpAuth/app/rest/buildTypes/id:mybuildss/builds/"">
<build id=""1234"" buildTypeId=""mybuild"" number=""1.0.475"" status=""SUCCESS"" state=""finished"" href=""/httpAuth/app/rest/builds/id:1234"" webUrl=""http://teamcity.win.sys.7d/viewLog.html?buildId=1234&amp;buildTypeId=mybuilds""/>
<build id=""1235"" buildTypeId=""mybuild"" number=""1.0.474"" status=""SUCCESS"" state=""finished"" pinned=""true"" href=""/httpAuth/app/rest/builds/id:1235"" webUrl=""http://teamcity.win.sys.7d/viewLog.html?buildId=1235&amp;buildTypeId=mybuilds""/>
</builds>";

		[Test]
		public void CanParse()
		{
			var parser = new BuildListXmlParser(BuildListXml);
			var builds = parser.FromIdBackToLastPin(1234);

			Assert.That(builds, Is.Not.Null);
			Assert.That(builds.Ids, Is.Not.Null);
			Assert.That(builds.Ids, Is.Not.Empty);
			Assert.That(builds.PreviousPinnedBuildId, Is.EqualTo(1235));
		}
	}
}
