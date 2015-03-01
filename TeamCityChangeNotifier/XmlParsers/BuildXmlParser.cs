using System.Linq;
using System.Xml.Linq;

using TeamCityChangeNotifier.Models;

namespace TeamCityChangeNotifier.XmlParsers
{
	public class BuildXmlParser
	{
		private readonly XDocument buildDoc;

		public BuildXmlParser(string buildXml)
		{
			buildDoc = XDocument.Parse(buildXml);
		}

		public int? DepenedencyBuildId()
		{
			var deps = buildDoc.Descendants("artifact-dependencies").FirstOrDefault();
			if (deps == null)
			{
				return null;
			}

			var firstBuild = deps.Descendants("build").FirstOrDefault();
			if (firstBuild == null)
			{
				return null;
			}

			var idValue = firstBuild.Attribute("id").Value;

			int intId;
			var parsedId = int.TryParse(idValue, out intId);
			if (!parsedId)
			{
				return null;
			}
			return intId;
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

		public BuildData GetBuildData()
		{
			return new BuildData
				{
					BuildType = BuildType(),
					ProjectName = ProjectName(),
					// Id = ?
					// DateTime = ?
					DependencyBuildId = DepenedencyBuildId()
				};
		}

		public string Text
		{
			get { return buildDoc.ToString(); }
		}
	}
}
