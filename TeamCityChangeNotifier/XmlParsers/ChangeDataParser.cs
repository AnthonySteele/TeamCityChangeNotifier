using System;
using System.Linq;
using System.Xml.Linq;

using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class ChangeDataParser
	{
		public ChangeData ReadXml(string changeXml)
		{
			var changeDoc = XDocument.Parse(changeXml);
			var root = changeDoc.Root;

			var id = ReadId(root);
			var comment = ReadComment(root);
			var userName = ReadUserName(root);
			var date = ReadDate(root);

			return new ChangeData
				{
					Id = id,
					Author = userName,
					Message = comment,
					Date = date
				};
		}

		private DateTime ReadDate(XElement root)
		{
			var dateAttr = root.Attribute("date").Value;
			return DateParser.Parse(dateAttr);
		}

		private static int ReadId(XElement root)
		{
			var idAttr = root.Attribute("id").Value;
			return int.Parse(idAttr);
		}

		private static string ReadComment(XElement root)
		{
			var commentNode = root.Descendants("comment").First();
			var comment = commentNode.Value;
			if (!string.IsNullOrEmpty(comment))
			{
				comment = comment.Trim();
			}
			return comment;
		}

		private static string ReadUserName(XElement root)
		{
			var userNode = root.Descendants("user").FirstOrDefault();
			if (userNode != null)
			{
				return userNode.Attribute("name").Value;
			}

			return root.Attribute("username").Value;
		}
	}
}