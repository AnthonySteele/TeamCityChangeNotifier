using NUnit.Framework;
using TeamCityChangeNotifier.XmlParsers;

namespace TeamCityChangeNotifier.Tests.XmlParsers
{
	[TestFixture]
	public class DateParserTests
	{
		[Test]
		public void TestParse()
		{
			var date = DateParser.Parse("20150218T114435+0000");

			Assert.That(date.Year, Is.EqualTo(2015));
			Assert.That(date.Month, Is.EqualTo(2));
			Assert.That(date.Day, Is.EqualTo(18));
			Assert.That(date.Hour, Is.EqualTo(11));
			Assert.That(date.Minute, Is.EqualTo(44));
			Assert.That(date.Second, Is.EqualTo(35));
		}
	}
}
