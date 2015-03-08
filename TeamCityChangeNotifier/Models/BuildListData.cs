using System.Collections.Generic;

namespace TeamCityChangeNotifier.Models
{
	public class BuildListData
	{
		public BuildData LatestBuild { get; set; }
		public int PreviousPinnedBuildId { get; set; }
		public BuildData PreviousPinned { get; set; }

		public List<int> Ids { get; set; }
	}
}
