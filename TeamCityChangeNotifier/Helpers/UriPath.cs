namespace TeamCityChangeNotifier.Helpers
{
	public static class UriPath
	{
		public static string Combine(string baseUri, string rest)
		{
			if (string.IsNullOrEmpty(baseUri))
			{
				return rest;
			}

			if (string.IsNullOrEmpty(rest))
			{
				return baseUri;
			}

			bool baseHasTrailingSlash = baseUri.EndsWith("/");
			bool restHasLeadingSlash = rest.StartsWith("/");

			if (baseHasTrailingSlash && restHasLeadingSlash)
			{
				return baseUri.Substring(0, baseUri.Length - 1) + rest;
			}

			if (baseHasTrailingSlash || restHasLeadingSlash)
			{
				return baseUri + rest;
			}

			return baseUri + "/" + rest;
		}

		public static string Combine(string baseUri, string part1, string part2)
		{
			return Combine(Combine(baseUri, part1), part2);
		}

		public static string Combine(string baseUri, string part1, string part2, string part3)
		{
			return Combine(Combine(baseUri, part1), Combine(part2, part3));
		}
	}
}