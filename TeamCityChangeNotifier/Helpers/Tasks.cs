using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamCityChangeNotifier.Helpers
{
	public static class Tasks
	{
		/// <summary>
		/// Not sure if this is great code or terrible code. But it works I think.
		/// It aims to extract out a acommon pattern that I use regularly
		/// i.e. given a list of inputs, and an async way to get outputs,
		/// Get the outputs in parallel
		/// e.g. In parallel, turn a list of Urls, (T is string) to responses 
		/// (U is HttpResponseMessage and the read func wraps a HTTP get)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="inputs"></param>
		/// <param name="read"></param>
		/// <returns></returns>
		public static async Task<List<U>> ReadParallel<T, U>(List<T> inputs, Func<T, Task<U>> read)
		{
			var readTasks = inputs.Select(read)
				.ToList();

			var results = await Task.WhenAll(readTasks);
			return results.ToList();
		}
	}
}
