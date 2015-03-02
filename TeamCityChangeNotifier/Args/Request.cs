namespace TeamCityChangeNotifier.Args
{
	public class Request
	{
		public bool ArgsError { get; set; }
		public string ArgsErrorMessage { get; set; }

		public int InitialBuildId { get; set; }

		public static Request Error(string message)
		{
			return new Request
			{
				ArgsError = true,
				ArgsErrorMessage =message
			};
		}
	}
}