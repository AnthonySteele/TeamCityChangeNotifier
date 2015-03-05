using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class BuildListXmlParser
	{
		private readonly XDocument buildDoc;

		public BuildListXmlParser(string buildXml)
		{
			buildDoc = XDocument.Parse(buildXml);
		}

		public BuildListData FromIdBackToLastPin(int firstBuildId)
		{
			var builds = buildDoc.Root.Descendants("build");
			var foundStartBuild = false;
			var foundEndBuild = false;
			var buildIds = new List<int>();
			int previousPinnedBuildId = 0;

			foreach (var buildElement in builds)
			{
				var buildId = BuildId(buildElement);

				if (foundStartBuild)
				{
					if ( BuildIsPinned(buildElement))
					{
						previousPinnedBuildId = buildId;
						foundEndBuild = true;
						break;
					}

					buildIds.Add(buildId);
				}
				else
				{

					if (buildId == firstBuildId)
					{
						buildIds.Add(buildId);
						foundStartBuild = true;
					}
				}
			}

			if (!foundStartBuild)
			{
				throw new ParseException("Did not find first build with Id " + firstBuildId);
			}

			if (!foundEndBuild)
			{
				throw new ParseException("Did not find previous pinned build after Id " + firstBuildId);
			}

			return new BuildListData
				{
					Ids = buildIds,
					PreviousPinnedBuildId = previousPinnedBuildId
				};
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
