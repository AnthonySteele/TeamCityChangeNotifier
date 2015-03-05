using System;

namespace TeamCityChangeNotifier.Models
{
	public class ChangeData
	{
		public int Id { get; set; }
		public string Author { get; set; }
		public string Message { get; set; }
		public DateTime Date { get; set; }

		public string Details()
		{
			return string.Format("{0} on {1:ddd} {1:g}{2}{3}", Author, Date, Environment.NewLine,  Message);
		}
	}
}
