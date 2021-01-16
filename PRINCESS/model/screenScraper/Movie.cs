using Nancy.Json;
using System;
using System.Net;

/** 
 *  authors:
 *          @Ali Asbai
 *          
 **/

namespace PRINCESS.model.screenScraper
{
    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public string Metascore { get; set; }
        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Response { get; set; }

        public static string getMovieID(string stitle)
        {
            string url = "http://www.omdbapi.com/?apikey=c585a26c&t=" + stitle.Trim();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                Movie obj = new Movie();
                obj = oJS.Deserialize<Movie>(json);
                if (obj.Response == "True")
                {
                    return obj.imdbID;
                }
                else
                {
                    return "0";
                }
            }
        }

        public static Movie getMovie(string simdbID)
        {
            string url = "http://www.omdbapi.com/?apikey=c585a26c&i=" + simdbID.Trim();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                JavaScriptSerializer oJS = new JavaScriptSerializer();
                Movie obj = new Movie();
                obj = oJS.Deserialize<Movie>(json);
                if (obj.Response == "True")
                {
                    return obj;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}
