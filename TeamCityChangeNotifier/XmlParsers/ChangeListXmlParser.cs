using System.Collections.Generic;
using System.Xml.Linq;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class ChangeListXmlParser
	{
		private readonly XDocument buildDoc;

		public ChangeListXmlParser(string buildXml)
		{
			buildDoc = XDocument.Parse(buildXml);
		}

		public List<string> ChangeHrefs()
		{
			var results = new List<string>();
			var changes = buildDoc.Root.Descendants("change");

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
