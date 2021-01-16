using System;

/**
 *  authors: 
 *          @Ali Asbai
 *          @Anna Mann
 *          @Gabriel Vega
 *          @Olivia Höft
 *      
 **/

namespace PRINCESS.model
{
    public class Review
    {
        public string Url { get; set; }

        public string Title { get; set; }
        public string ImdbID { get; set; }
        public string MovieID { get; set; }
        public string ReviewText { get; set; }
        public string Writer { get; set; }
        public string PublishDate { get; set; }
        public string DomainUrl { get; set; }
        public string Rating { get; set; }
        public int Priority { get; set; }
        public string ProductionCountry { get; set; }

        public Review(string url, string title, string reviewText, string writer, string publishDate)
        {
            if (reviewText == "" || writer == "" || publishDate == "") throw new ArgumentException();
            if (reviewText == null || writer == null || publishDate == null) throw new ArgumentException();
            this.Url = url;
            ImdbID = "";
            this.Title = title;
            this.MovieID = "";
            this.ReviewText = reviewText;
            this.Writer = writer;
            this.Rating = "";
            this.DomainUrl = "";
            this.PublishDate = publishDate;
        }

        public Review(string url, string movieID, string title, string reviewText, string writer, string publishDate, string rating, string domainUrl, int priority)
        {
            if (reviewText == "" || writer == "" || publishDate == "") throw new ArgumentException();
            if (reviewText == null || writer == null || publishDate == null) throw new ArgumentException();
            this.Url = url;
            ImdbID = "";
            this.Title = title;
            this.MovieID = movieID;
            this.ReviewText = reviewText;
            this.Writer = writer;
            this.Rating = rating;
            this.DomainUrl = domainUrl;
            this.PublishDate = publishDate;
            this.Priority = priority;
        }
    }
}
