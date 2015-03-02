using System;
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
			var buildTypeId = buildDoc.Root.Attribute("buildTypeId");
			return buildTypeId.Value;
		}

		public int Id()
		{
			var idValue = buildDoc.Root.Attribute("id").Value;
			return int.Parse(idValue);
		}


		public string ProjectName()
		{
			var buildType = buildDoc.Root.Descendants("buildType").First();
			return buildType.Attributes("projectName").First().Value;
		}

		private DateTime FinishDate()
		{
			var finishDate = buildDoc.Root.Descendants("finishDate").First();
			var dateText = finishDate.Value;
			return DateParser.Parse(dateText);
		}

		private DateTime StartDate()
		{
			var finishDate = buildDoc.Root.Descendants("startDate").First();
			var dateText = finishDate.Value;
			return DateParser.Parse(dateText);
		}
		
		public BuildData GetBuildData()
		{
			return new BuildData
				{
					Id = Id(),
					StartDate = StartDate(),
					FinishDate = FinishDate(),
					BuildType = BuildType(),
					ProjectName = ProjectName(),
					DependencyBuildId = DepenedencyBuildId()
				};
		}

		public string Text
		{
			get { return buildDoc.ToString(); }
		}
	}
}
