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

        public async Task<IEnumerable<string>> GetGenres(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("recommendations/available-genre-seeds");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var genresResult = await JsonSerializer.DeserializeAsync<GetGenresResult>(responseStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ensure case-insensitivity for JSON property names
            });

            return genresResult?.Genres ?? Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<Release>> GetRecommendations(string genre, int limit, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"recommendations?seed_artists=4NHQUGzhtTLFvgF5SZesLK&seed_genres={genre}&limit={limit}&seed_tracks=0c6xIDDpzE81m2q797ordA");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<GetRecommendationResult>(responseStream);

            return responseObject?.tracks?.Select(item => new Release
            {
                Name = item.album.name,
                Date = item.album.release_date, 
                ImageUrl = item.album.images.FirstOrDefault()?.url,
                Link = item.album.external_urls.spotify,
                Artists = string.Join(", ", item.album.artists.Select(artist => artist.name))
            }) ?? Enumerable.Empty<Release>();
        }
    }
}
