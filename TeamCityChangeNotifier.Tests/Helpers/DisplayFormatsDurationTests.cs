using System;

using NUnit.Framework;

using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Tests.Helpers
{
	[TestFixture]
	public class DisplayFormatsDurationTests
	{
		[Test]
		public void OneHourBetween()
		{
			var now = DateTime.Now;

			var betweenText = DisplayFormats.Between(now, now.AddHours(1));

			Assert.That(betweenText, Is.EqualTo("1 hour"));
		}

		[Test]
		public void TenHoursBetween()
		{
			var now = DateTime.Now;

			var betweenText = DisplayFormats.Between(now, now.AddHours(10));

			Assert.That(betweenText, Is.EqualTo("10 hours"));
		}

		[Test]
		public void OneDayBetween()
		{
			var now = DateTime.Now;

			var betweenText = DisplayFormats.Between(now, now.AddDays(1));

			Assert.That(betweenText, Is.EqualTo("1 day"));
		}

		[Test]
		public void TenDaysBetween()
		{
			var now = DateTime.Now;

			var betweenText = DisplayFormats.Between(now, now.AddDays(10));

			Assert.That(betweenText, Is.EqualTo("10 days"));
		}
	}
}
