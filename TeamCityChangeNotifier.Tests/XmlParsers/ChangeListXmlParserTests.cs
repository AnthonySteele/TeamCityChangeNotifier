using NUnit.Framework;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Tests.XmlParsers
{
	[TestFixture]
	public class ChangeListXmlParserTests
	{
		private const string ChangeListXml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<changes count=""3"" href=""/httpAuth/app/rest/changes?locator=build:(id:209507)"">
	<change id=""123"" version=""feb7926dc34ca9d0d57a49a744e138792a618a87"" username=""anthony"" date=""20150218T114224+0000"" href=""/httpAuth/app/rest/changes/id:123"" webLink=""http://teamcity.win.sys.7d/viewModification.html?modId=123&amp;personal=false""/>
	<change id=""234"" version=""251a60e98f1f781fc5baaf3a15d0bf9d2cf4481d"" username=""anthony"" date=""20150218T111011+0000"" href=""/httpAuth/app/rest/changes/id:234"" webLink=""http://teamcity.win.sys.7d/viewModification.html?modId=234&amp;personal=false""/>
	<change id=""456"" version=""c41f7ab917f10122e8bd33a0f5d61ed0dc5afbf8"" username=""anthony"" date=""20150218T102616+0000"" href=""/httpAuth/app/rest/changes/id:456"" webLink=""http://teamcity.win.sys.7d/viewModification.html?modId=345&amp;personal=false""/>
</changes>";


		[Test]
		public void CanParseChanges()
		{
			var changes = new ChangeListXmlParser(ChangeListXml);

			var hrefs = changes.ChangeHrefs();

			Assert.That(hrefs, Is.Not.Null);
			Assert.That(hrefs.Count, Is.EqualTo(3));
		}
	}
}
