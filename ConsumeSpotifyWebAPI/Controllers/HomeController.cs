using ConsumeSpotifyWebAPI.Models;
using ConsumeSpotifyWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsumeSpotifyWebAPI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISpotifyAccountService _spotifyAccountService;
		private readonly IConfiguration _configuration;
		private readonly ISpotifyService _spotifyService;

		public HomeController(ISpotifyAccountService spotifyAccountService, IConfiguration configuration, ISpotifyService spotifyService)
		{
			_spotifyAccountService = spotifyAccountService;	
			_configuration = configuration;
			_spotifyService = spotifyService;
		}

		public async Task<IActionResult> Index(string tab, string genre)
		{

            string accessToken = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
            int limit = 20;

            if (tab == "recommendations")
            {
                ViewData["Tab"] = "recommendations";

                var genres = await _spotifyService.GetGenres(accessToken);
                ViewData["Genres"] = genres;

                var recommendations = genre != null
                    ? await _spotifyService.GetRecommendations(genre, limit, accessToken)
                    : Enumerable.Empty<Release>();

                return View(recommendations);
            }
            else
            {
                ViewData["Tab"] = "new-releases";

                // New Releases
                var releases = await GetReleases(limit, accessToken);
                return View(releases);
            }
        }

        public async Task<IEnumerable<Release>> GetReleases(int limit, string token)
		{
			try
			{
				var newReleases = await _spotifyService.GetNewReleases(limit, token);	

				return newReleases;
			}
			catch(Exception ex)
			{
				Debug.Write(ex);
			}
			return Enumerable.Empty<Release>();	
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
