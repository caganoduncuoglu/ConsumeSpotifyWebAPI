namespace ConsumeSpotifyWebAPI.Models
{
   
        public class GetRecommendationResult
        {
            public Seed[] seeds { get; set; }
            public Track[] tracks { get; set; }
        }

        public class Seed
        {
            public int afterFilteringSize { get; set; }
            public int afterRelinkingSize { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public int initialPoolSize { get; set; }
            public string type { get; set; }
        }

        public class Track
        {
            public Album album { get; set; }
            public string[] available_markets { get; set; }
            public int disc_number { get; set; }
            public int duration_ms { get; set; }
            public bool _explicit { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string preview_url { get; set; }
            public int track_number { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
            public bool is_local { get; set; }
        }

        public class Album
        {
            public string album_type { get; set; }
            public int total_tracks { get; set; }
            public string[] available_markets { get; set; }
            public External_Urls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public Image[] images { get; set; }
            public string name { get; set; }
            public string release_date { get; set; }
            public string release_date_precision { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
            public Artist[] artists { get; set; }
        }

}

