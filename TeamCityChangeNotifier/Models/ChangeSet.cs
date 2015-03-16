using System.Collections.Generic;
using System.Text;

using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Models
{
	public class ChangeSet
	{
		public List<ChangeData> Changes { get; set; }
		public BuildListData Builds { get; set; }

		public BuildData ReleaseBuild { get; set; }

		public string Summary()
		{
			var elapsed = DisplayFormats.Between(Builds.LatestBuild.FinishDate, Builds.PreviousPinned.FinishDate);

			return string.Format("Release to {0} with {1} over {2}", 
				ReleaseBuild.ProjectName, ChangeCount(), elapsed);
		}

		public string Details()
		{
			var result = new StringBuilder();
			result.AppendLine(Summary());
			result.AppendFormat("Finished at {0:f}\r\n", ReleaseBuild.FinishDate);

			result.AppendLine();
			foreach (var change in Changes)
			{
				result.AppendLine(change.Details());
				result.AppendLine();
			}

			return result.ToString().Trim();
		}

		private string ChangeCount()
		{
			return DisplayFormats.Count(Changes.Count, "change");
		}

		private string BuildCount()
		{
			return DisplayFormats.Count(Builds.Ids.Count, "build");
		}
	}
}