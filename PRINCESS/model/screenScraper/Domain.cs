using System;
using System.Runtime.Serialization;

/**
 *  authors:
 *          @Ali Asbai
 *          @Clara Hällgren
 *          @Olivia Höft
 *          
 **/

namespace PRINCESS.model.screenScraper
{
    [DataContract]
    public class Domain
    {
        [DataMember]
        public HtmlElement title;
        [DataMember]
        public HtmlElement review;
        [DataMember]
        public HtmlElement writer;
        [DataMember]
        public HtmlElement rating;
        [DataMember]
        public HtmlElement publishDate;
        [DataMember]
        public HtmlElement menu;
        [DataMember]
        public string loginButton { get; set; }
        [DataMember]
        public string nextButton;
        [DataMember]
        public string LastScrape;
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string password { get; set; }

        private int priority;
        private string URL, domainURL, country;

        public Domain()
        {
            this.title = new HtmlElement("//");
            this.review = new HtmlElement("//");
            this.writer = new HtmlElement("//");
            this.rating = new HtmlElement("//");
            this.publishDate = new HtmlElement("//");
            this.menu = new HtmlElement("//");

            country = "";
            setURL("");
            setDomainURL("");
            this.LastScrape = "FIRST_SCRAPE_RUN";
            this.priority = 1;
            country = "";
            nextButton = "DOMAIN_ONLY_HAS_1_PAGE";
            email = "";
            password = "";
            loginButton = "";
        }
        public Domain(HtmlElement title, HtmlElement writer, HtmlElement review, HtmlElement rating, HtmlElement publishDate, HtmlElement menu, string domainURL, string LastScrape, string password, string email)
        {
            this.title = title;
            this.review = review;
            this.writer = writer;
            this.rating = rating;
            this.publishDate = publishDate;
            this.menu = menu;
            setURL(domainURL);
            setDomainURL(domainURL);
            this.LastScrape = "FIRST_SCRAPE_RUN";
            country = "";
            this.priority = 0;
            nextButton = "DOMAIN_ONLY_HAS_1_PAGE";
            email = "";
            password = "";
            loginButton = "";
        }

        public HtmlElement getActiveElement(string activeElement)
        {
            switch (activeElement)
            {
                case "Title" :
                    return getTitle();
                case "Review":
                    return getReview();
                case "Writer":
                    return getWriter();
                case "Rating":
                    return getRating();
                case "PublishDate":
                    return getPublishDate();
                default:
                    throw new ArgumentException();
            }
        }

        public HtmlElement getTitle()
        {
            return title;
        }

        public void setTitle(HtmlElement title)
        {
            this.title = title;
        }

        public HtmlElement getWriter()
        {
            return writer;
        }

        public void setWriter(HtmlElement writer)
        {
            this.writer = writer;
        }

        public HtmlElement getReview()
        {
            return review;
        }

        public void setReview(HtmlElement review)
        {
            this.review = review;
        }

        public HtmlElement getRating()
        {
            return rating;
        }

        public void setRating(HtmlElement rating)
        {
            this.rating = rating;
        }

        public string getLastScrape()
        {
            return LastScrape;
        }

        public void setLastScrape(string link)
        {
            LastScrape = link;
        }

        public string getCountry()
        {
            return country;
        }

        public void setCountry(string country)
        {
            this.country = country;
        }

        public int getPriority()
        {
            return priority;
        }

        public void setPriority(int priority)
        {
            this.priority = priority;
        }

        public HtmlElement getPublishDate()
        {
            return publishDate;
        }

        public void setPublishDate(HtmlElement publishDate)
        {
            this.publishDate = publishDate;
        }

        public HtmlElement getMenu()
        {
            return menu;
        }

        public void setMenu(HtmlElement menu)
        {
            this.menu = menu;
        }

        public void setNextButton(string nextButton)
        {
            this.nextButton = nextButton;
        }

        public string getNextButton()
        {
            return nextButton;
        }

        public string getURL()
        {
            return URL;
        }

        public void setURL(string URL)
        {
            this.URL = URL;
        }

        public string getDomainURL()
        {
            return domainURL;
        }

        public void setDomainURL(string domainURL)
        {
            this.domainURL = domainURL;
        }

        public void setEmail(string email)
        {
            this.email = email;
        }

        public string getEmail()
        {
            return this.email;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public string getPassword()
        {
            return this.password;
        }
    }
}
