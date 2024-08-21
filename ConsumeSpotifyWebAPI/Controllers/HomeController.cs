using ConsumeSpotifyWebAPI.Models;
using ConsumeSpotifyWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
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

		public async Task<IActionResult> Index()
		{
			var releases = await GetReleases();

			return View(releases);
		}		

		public async Task<IEnumerable<Release>> GetReleases()
		{
			try
			{
				var token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
			
				var newReleases = await _spotifyService.GetNewReleases(20, token);	

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
