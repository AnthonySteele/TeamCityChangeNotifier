using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TeamCityChangeNotifier.Http
{
	public class HttpReader
	{
		private readonly HttpClient _client = new HttpClient();
		private readonly TeamCityAuth _authInfo;

		public HttpReader(TeamCityAuth authInfo)
		{
			_authInfo = authInfo;
		}

		public async Task<string> ReadResponseBody(string url)
		{
			var response = await ReadUrl(url);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}

		private async Task<HttpResponseMessage> ReadUrl(string url)
		{

			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Authorization = MakeBasicAuthHeader();

			return await _client.SendAsync(request);
		}

		private AuthenticationHeaderValue MakeBasicAuthHeader()
		{
			return new AuthenticationHeaderValue("Basic", _authInfo.AuthInfo());
		}
	}
}