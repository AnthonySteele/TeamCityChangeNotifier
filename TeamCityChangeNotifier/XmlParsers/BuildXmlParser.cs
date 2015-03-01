using System.Linq;
using System.Xml.Linq;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class BuildXmlParser
	{
		private readonly XDocument buildDoc;

		public BuildXmlParser(string buildXml)
		{
			buildDoc = XDocument.Parse(buildXml);
		}

		public int FirstBuildId()
		{
			var deps = buildDoc.Descendants("artifact-dependencies").First();
			var dep = deps.Descendants("build").First();
			var idValue = dep.Attributes("id").First().Value;
			return int.Parse(idValue);
		}

		public string BuildType()
		{
			var buildTypeId = buildDoc.Root.Attributes("buildTypeId").First();
			return buildTypeId.Value;
		}

		public string ProjectName()
		{
			var buildType = buildDoc.Root.Descendants("buildType").First();
			return buildType.Attributes("projectName").First().Value;
		}


		public string Text
		{
			get { return buildDoc.ToString(); }
		}
	}
}
