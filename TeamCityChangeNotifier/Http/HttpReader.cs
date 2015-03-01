using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TeamCityChangeNotifier.Helpers;

namespace TeamCityChangeNotifier.Http
{
	public class HttpReader
	{
		private readonly Settings settings = new Settings();

		public async Task<string> ReadResponseBody(string url)
		{
			var response = await ReadUrl(url);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}

		private async Task<HttpResponseMessage> ReadUrl(string url)
		{
			HttpClient client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			SetBasicAuthHeader(request);
			return await client.SendAsync(request);
		}

		public void SetBasicAuthHeader(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Basic", settings.TeamcityAuthInfo);
		}
	}
}