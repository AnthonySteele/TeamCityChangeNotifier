using System;

namespace TeamCityChangeNotifier.Helpers
{
	public static class DisplayFormats
	{
		public static string Count(int count, string word)
		{
			if (count == 0)
			{
				return "No " + word + "s";
			}

			return string.Format("{0} {1}", count, Pluralise(word, count));
		}

		private static string Pluralise(string word, int count)
		{
			return word + Pluralise(count);
		}

		private static string Pluralise(int count)
		{
			return (count == 1) ? "" : "s";
		}

		public static string Between(DateTime from, DateTime to)
		{
			var elapsed = (to - from).Duration();

			if (elapsed.TotalDays >= 1)
			{
				int days = (int)Math.Floor(elapsed.TotalDays);
				return days + "day" + Pluralise(days);
			}

			int hours = (int)Math.Floor(elapsed.TotalHours);
			return hours + "day" + Pluralise(hours);
		}
	}
}
