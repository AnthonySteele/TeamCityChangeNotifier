using System;

namespace TeamCityChangeNotifier.Models
{
	public class BuildData
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime FinishDate { get; set; }

		public string BuildType { get; set; }
		public string ProjectName { get; set; }

		public int? DependencyBuildId { get; set; }
	}
}