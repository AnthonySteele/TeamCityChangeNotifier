using System;
using NUnit.Framework;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Tests.XmlParsers
{
	[TestFixture]
	public class ChangeDataParserTests
	{
		private const string BuildXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
	<change id=""12345"" version=""feb7926dc34ca9d0d57a49a744e138792a618a87"" username=""anthony"" date=""20150218T114224+0000"" href=""/httpAuth/app/rest/changes/id:12345"" webLink=""http://teamcity.win.sys.7d/viewModification.html?modId=12345&amp;personal=false""><comment>This is a change</comment>
	<files><file before-revision=""251a60e98f1f781fc5baaf3a15d0bf9d2cf4481d"" after-revision=""feb7926dc34ca9d0d57a49a744e138792a618a87"" file=""src/foo.cs"" relative-file=""src/foo.cs""/>
	</files>
	<vcsRootInstance id=""234"" vcs-root-id=""myVcs"" name=""mybuild"" href=""/httpAuth/app/rest/vcs-root-instances/id:1234""/></change>";

		[Test]
		public void CanParseChanges()
		{
			var parser = new ChangeDataParser();

			var changeData = parser.ReadXml(BuildXml);

			Assert.That(changeData, Is.Not.Null);
			Assert.That(changeData.Id, Is.EqualTo(12345));
			Assert.That(changeData.Author, Is.EqualTo("anthony"));
			Assert.That(changeData.Message, Is.EqualTo("This is a change"));
			Assert.That(changeData.Date, Is.EqualTo(new DateTime(2015, 2, 18, 11, 42,24)));
		}
	}
}
