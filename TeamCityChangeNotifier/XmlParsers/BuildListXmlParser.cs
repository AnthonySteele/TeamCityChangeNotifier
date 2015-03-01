using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class BuildListXmlParser
	{
		private readonly XDocument buildDoc;

		public BuildListXmlParser(string buildXml)
		{
			buildDoc = XDocument.Parse(buildXml);
		}

		public List<int> FromIdBackToLastPin(int firstBuildId)
		{
			var builds = buildDoc.Root.Descendants("build");
			var found = false;
			var buildIds = new List<int>(); 

			foreach (var buildElement in builds)
			{
				var buildId = BuildId(buildElement);

				if (found)
				{
					if ( BuildIsPinned(buildElement))
					{
						break;
					}

					buildIds.Add(buildId);
				}
				else
				{

					if (buildId == firstBuildId)
					{
						buildIds.Add(buildId);
						found = true;
					}
				}
			}

			return buildIds;
		}

		private static int BuildId(XElement buildElement)
		{
			var iddAttr = buildElement.Attributes("id").First().Value;
			return int.Parse(iddAttr);
		}

		private static bool BuildIsPinned(XElement buildElement)
		{
			var pinnedAttr = buildElement.Attributes("pinned").FirstOrDefault();
			return (pinnedAttr != null) && (pinnedAttr.Value == "true");
		}
	}
}
