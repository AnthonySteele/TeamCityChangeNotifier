using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeamCityChangeNotifier.Http
{
	public class HttpReader
	{
		private readonly TeamCityAuth authInfo = new TeamCityAuth();

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
			request.Headers.Authorization = MakeBasicAuthHeader();
			
			return await client.SendAsync(request);
		}

		private AuthenticationHeaderValue MakeBasicAuthHeader()
		{
			return new AuthenticationHeaderValue("Basic", authInfo.AuthInfo());
		}
	}
}