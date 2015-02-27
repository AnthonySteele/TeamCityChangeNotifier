using System.Linq;
using System.Xml.Linq;

namespace TeamCityChangeNotifier.Parsers
{
	public class BuildXmlParser
	{
		private readonly XDocument _buildDoc;

		public BuildXmlParser(string buildXml)
		{
			_buildDoc = XDocument.Parse(buildXml);
		}

		public int FirstBuildId()
		{
			var deps = _buildDoc.Descendants("artifact-dependencies").First();
			var dep = deps.Descendants("build").First();
			var idValue = dep.Attributes("id").First().Value;
			return int.Parse(idValue);
		}

		public string BuildType()
		{
			var buildTypeId = _buildDoc.Root.Attributes("buildTypeId").First();
			return buildTypeId.Value;
		}

		public string ProjectName()
		{
			var buildType = _buildDoc.Root.Descendants("buildType").First();
			return buildType.Attributes("projectName").First().Value;
		}


		public string Text
		{
			get { return _buildDoc.ToString(); }
		}
	}
}
