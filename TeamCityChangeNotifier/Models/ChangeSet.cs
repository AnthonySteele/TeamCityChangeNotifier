using System.Collections.Generic;
using System.Text;

using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Models
{
	public class ChangeSet
	{
		public List<ChangeData> Changes { get; set; }
		public int PinnedBuildId { get; set; }
		public BuildListData Builds { get; set; }

		public BuildData ReleaseBuild { get; set; }

		public string Summary()
		{
			return string.Format("Release to {0} with {1} in {2}", ReleaseBuild.ProjectName, ChangeCount(), BuildCount());
		}

		public string Details()
		{
			var result = new StringBuilder();
			result.AppendLine(Summary());
			result.AppendLine();
			foreach (var change in Changes)
			{
				result.AppendLine(change.Details());
				result.AppendLine();
			}

			return result.ToString();
		}

		private string ChangeCount()
		{
			return DisplatFormats.Count(Changes.Count, "change");
		}

		private string BuildCount()
		{
			return DisplatFormats.Count(Builds.Ids.Count, "build");
		}
	}
}