using NUnit.Framework;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Tests.Helpers
{
	[TestFixture]
	public class DisplayFormatsCountsTests
	{
		[Test]
		public void TestCount1()
		{
			var text = DisplayFormats.Count(1, "foo");

			Assert.That(text, Is.EqualTo("1 foo"));
		}

		[Test]
		public void TestCount2()
		{
			var text = DisplayFormats.Count(2, "foo");

			Assert.That(text, Is.EqualTo("2 foos"));
		}

		[Test]
		public void TestCount0()
		{
			var text = DisplayFormats.Count(0, "foo");

			Assert.That(text, Is.EqualTo("no foos"));
		}
	}
}
