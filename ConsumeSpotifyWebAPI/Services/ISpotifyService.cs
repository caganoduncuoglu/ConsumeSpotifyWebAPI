using ConsumeSpotifyWebAPI.Models;

namespace ConsumeSpotifyWebAPI.Services
{
	public interface ISpotifyService
	{
		Task<IEnumerable<Release>> GetNewReleases(int limit, string accessToken);
	}
}
