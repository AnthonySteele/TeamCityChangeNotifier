using System;
using System.Globalization;

namespace TeamCityChangeNotifier.XmlParsers
{
	public static class DateParser
	{
		public static DateTime Parse(string value)
		{
			var dateTime = value.Substring(0, value.IndexOf("+"));

			var provider = CultureInfo.InvariantCulture;
			return DateTime.ParseExact(dateTime, "yyyyMMddTHHmmss", provider);
		}
	}
}
