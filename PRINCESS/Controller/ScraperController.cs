using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PRINCESS.model;
using static PRINCESS.model.screenScraper.ReviewScraper;
using PRINCESS.model.screenScraper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PRINCESS.controller.PrincessDb;
using static PRINCESS.model.screenScraper.Movie;

/**
 *  authors:
 *      @Olivia Höft
 *      @Ali Asbai
 *      @Clara Hällgren
 *      @Anna Mann
 *
 *  A controller class for screenScraping movie reviews online.
 *
 *  public static void collectReviews()
 *  Fetches all new reviews across all domains in the database and stores them in the Review table.
 *
 *  public static void insertReviewsDb(List<Review> reviews)
 *  Executes insert calls for all reviews in a given list.
 *  parameters:
 *      reviews - A list of Review objects.
 *
 *  public static List<Review>getNewReviews(List<Domain> domains)
 *  Finds new links for each domain in a given list and creates Review objects out of them.
 *  parameters:
 *      domains - A list of domains to explore.
 *  return:
 *      reviews - A list of new Review objects.
 *
 *  public static List<Review> createReviews(List<string> links, Domain domain, IWebDriver driver)
 *  Creates a Review list based on links from a given list.
 *  parameters:
 *      links - A list of links to explore.
 *      domain - The domain instance of a website that contains movie reviews.
 *      driver - The selenium driver that navigates between the elements in links.
 *  return:
 *      reviews - A list of new Review objects.
**/

namespace PRINCESS.controller
{
    public class ScraperController
    {
        public static void collectReviews()
        {
            Task<List<Domain>> select = Task.FromResult(getDomains());
            Task<List<Review>> reviews = Task.FromResult(getNewReviews(select.Result));
            Task insert = new Task(() => new Action<List<Review>>(insertReviewsDb)(reviews.Result));

            insert.Start();
            insert.Wait();
            Task.WaitAll();
        }

        public static void insertReviewsDb(List<Review> reviews)
        {
            foreach (Review r in reviews)
            {
                insertReview(r);
            }
        }

        public static List<Review> getNewReviews(List<Domain> domains)
        {
            List<string> links;
            List<Review> reviews = new List<Review>();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArguments("headless");
            IWebDriver driver = new ChromeDriver(options);
            foreach (Domain d in domains)
            {
                links = new List<string>();
                try
                {
                    LoadNewPage(d, driver);
                    links = navigateScrape(d, driver);
                    reviews.AddRange(createReviews(links, d, driver));
                    Task alter = new Task(() => new Action<Domain>(alterDomain)(d));
                    alter.Start();
                    alter.Wait();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return reviews;
        }


        public static List<Review> createReviews(List<string> links, Domain domain, IWebDriver driver)
        {
            List<Review> reviews = new List<Review>();

            domain.setURL(links[0]);
            if(domain.email != "" && domain.password != "")logIn(domain, driver);

            foreach (string l in links)
            {
                domain.setURL(l);
                try
                {
                    LoadNewPage(domain, driver);
                    reviews.Add(
                    new Review(l,
                    ReadScrape("Title", domain, driver),
                    ReadScrape("Review", domain, driver),
                    ReadScrape("Writer", domain, driver),
                    ReadScrape("PublishDate", domain, driver)
                    ));

                    reviews[reviews.Count - 1].ImdbID = getMovieID(reviews[reviews.Count - 1].Title);
                    reviews[reviews.Count - 1].Rating = scrapeRating(domain, driver);
                    reviews[reviews.Count - 1].DomainUrl = domain.getDomainURL();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            return reviews;
        }
    }
}
