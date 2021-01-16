using NUnit.Framework;
using PRINCESS.model.screenScraper;
using System;

namespace PRINCESS_Test
{
    public class ScreenScraperTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("//a[@class='author-box__name'])")]
        [TestCase(null)]
        [TestCase(" ")]
        public void HtmlElement_null_exception(string s)
        {
            /*ReviewScraper r = new ReviewScraper("https://www.dn.se/kultur-noje/cate-blanchett-ar-magnifik-i-mrs-america/");
            Console.WriteLine("Writer: " + r.ReadScrape(writer));*/
            Assert.Throws(typeof(ArgumentNullException), new TestDelegate(htmlElemenConst);
        }

        void htmlElemenConst(string s)
        {
            HtmlElement n = new HtmlElement(s);
        }
    }
}