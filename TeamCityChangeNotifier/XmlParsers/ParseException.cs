using System;

namespace TeamCityChangeNotifier.XmlParsers
{
	[Serializable]
	public class ParseException : Exception
	{
		public ParseException()
		{
		}

		public ParseException(string message) : base(message)
		{
		}
	}
}
