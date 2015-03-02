using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamCityChangeNotifier.Helpers
{
	public static class Tasks
	{
		public static async Task<List<U>> ReadParallel<T, U>(List<T> inputs, Func<T, Task<U>> read)
		{
			var readTasks = inputs.Select(read)
				.ToList();

			var results = await Task.WhenAll(readTasks);
			return results.ToList();
		}
	}
}
