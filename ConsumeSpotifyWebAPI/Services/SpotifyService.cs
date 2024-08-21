using ConsumeSpotifyWebAPI.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ConsumeSpotifyWebAPI.Services
{
	public class SpotifyService : ISpotifyService
	{
		private readonly HttpClient _httpClient;

		public SpotifyService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<Release>> GetNewReleases(int limit, string accessToken)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var response = await _httpClient.GetAsync($"browse/new-releases?limit={limit}");	

			response.EnsureSuccessStatusCode();

			using var responseStream = await response.Content.ReadAsStreamAsync();
			var responseObject = await JsonSerializer.DeserializeAsync<GetNewReleaseResult>(responseStream);

			return responseObject?.albums?.items.Select(item => new Release
			{
				Name = item.name,
				Date = item.release_date,
				ImageUrl = item.images.FirstOrDefault().url,
				Link = item.external_urls.spotify,
				Artists = string.Join(", ", item.artists.Select(item => item.name))
			});	
		}
	}
}
