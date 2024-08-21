using ConsumeSpotifyWebAPI.Models;

namespace ConsumeSpotifyWebAPI.Services
{
	public interface ISpotifyService
	{
		Task<IEnumerable<Release>> GetNewReleases(int limit, string accessToken);
        Task<IEnumerable<string>> GetGenres(string accessToken);
        Task<IEnumerable<Release>> GetRecommendations(string genre, int limit, string accessToken);
    }
}
