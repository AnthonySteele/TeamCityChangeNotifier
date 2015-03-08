using System.Collections.Generic;
using System.Xml.Linq;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class ChangeListXmlParser
	{
		private readonly XDocument _buildDoc;

		public ChangeListXmlParser(string buildXml)
		{
			_buildDoc = XDocument.Parse(buildXml);
		}

		public List<string> ChangeHrefs()
		{
			var results = new List<string>();
			var changes = _buildDoc.Root.Descendants("change");

			foreach (var changeElement in changes)
			{
				var hrefAttr = changeElement.Attribute("href");
				if (hrefAttr != null)
				{
					results.Add(hrefAttr.Value);
				}
			}

			return results;
		}
	}
}
