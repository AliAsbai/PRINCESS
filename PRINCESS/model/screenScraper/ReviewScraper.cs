using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Threading;

/**
 *  authors:
 *      @Olivia Höft
 *      @Ali Asbai
 *      @Clara Hällgren
 *      @Anna Mann
 *
 *  A class for screenscraping movie reviews online.
 *
 *  public static void LoadNewPage(Domain domain, IWebDriver driver)
 *  Navigates the given WebDriver to the next link.
 *  parameters:
 *      domain - Contains the new URL.
 *      driver - The selenium driver that navigates between the elements in links.
 *
 *  public static string ReadScrape(String activeElement, Domain domain, IWebDriver driver)
 *  Read key data from a website based on a given Xpath.
 *  parameters:
 *      activeElement - The name of the data being read.
 *      domain - Contains the Xpath of the active element.
 *      driver - The selenium driver that navigates between the elements in links.
 *  return:
 *      result - The scraped data in the form of a string.
 *
 *  public static List<string> navigateScrape(Domain domain, IWebDriver driver)
 *  Navigates the menu of a domain to find new review links.
 *  parameters:
 *      domain - Holds the Xpath of the menu containing the links.
 *      driver - The selenium driver that navigates between the elements in links.
 *  return:
 *      links - A list of new review links.
 *
 *  public static void pressNext(Domain domain, IWebDriver driver)
 *  Navigates to the next page of a menu.
 *  parameters:
 *      domain - Holds the Xpath of the next page button.
 *      driver - The selenium driver that navigates between the elements in links.
**/
namespace PRINCESS.model.screenScraper
{
    public static class ReviewScraper
    {
        public static void LoadNewPage(Domain domain, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(domain.getURL());
            Thread.Sleep(2000);
        }

        public static string ReadScrape(String activeElement, Domain domain, IWebDriver driver)
        {
            string result = "";
            string element = domain.getActiveElement(activeElement).GetElement();
            ReadOnlyCollection<IWebElement> text = driver.FindElements(By.XPath(element));
            foreach (IWebElement p in text)
            {
                result = string.Concat(result, p.GetAttribute("innerText").Trim());
                if (text.IndexOf(p) != text.Count - 1) result = string.Concat(result, "\n");
            }

            if (domain.getActiveElement(activeElement).GetRemoveCharsProp()) result = domain.getActiveElement(activeElement).RemoveChars(result);
            result = result.Replace("&nbsp;", "").Trim();
            result = result.Replace("&quot;", "\"").Trim();

            return result;
        }

        public static string scrapeRating(Domain domain, IWebDriver driver)
        {
            //logIn(d, "", driver); // hmm hur kan vi logg in för varje gång??

            string result = "";
            ReadOnlyCollection<IWebElement> text = driver.FindElements(By.XPath(domain.getRating().GetElement()));
            try
            {
                result = text[0].GetAttribute("class");
                if(domain.getRating().GetRemoveCharsProp()) result = domain.getRating().RemoveChars(result);
                if (!(result.Length > 0) || !(result.Length < 4)) throw new Exception();
                return result;
            }
            catch (Exception ex)
            {
                int i = 0;
                foreach (IWebElement p in text)
                {
                    i++;
                }
                return i.ToString();
            }
        }

        public static List<string> navigateScrape(Domain domain, IWebDriver driver)
        {
            List<string> links = new List<string>();
            string element = domain.getMenu().GetElement();
            ReadOnlyCollection<IWebElement> result = null;
            string h = "";
            int counter = 0;
            while (h != domain.getLastScrape() && counter < 5)
            {
                try
                {
                    result = driver.FindElements(By.XPath(element));
                    if(counter == 0)domain.setLastScrape(result[0].GetAttribute("href"));
                    foreach (IWebElement a in result)
                    {
                        try
                        {
                            h = a.GetAttribute("href");
                            if (domain.getMenu().GetIgnoreTitleKeyProp())
                            {
                                if (!domain.getMenu().CheckIgnoredKey(a.GetAttribute("innerText"))) links.Add(h);
                            }
                            else
                            {
                                links.Add(h);
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Invalid link read at exception: " + ex.ToString());
                        }
                    }
                    if (domain.getNextButton() != "DOMAIN_ONLY_HAS_1_PAGE")
                    {
                        pressNext(domain, driver);
                        counter++;
                    }
                    else
                    {
                        counter = 5;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid element at exception: " + ex.ToString()); // <--exception
                }
            }
            return links;
        }

        public static void pressNext(Domain domain, IWebDriver driver)
        {
            var result = driver.FindElement(By.XPath(domain.getNextButton()));
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].click()", result);
        }

        public static bool logIn(Domain d, IWebDriver driver)
        {
            //driver.Navigate().GoToUrl(l);
            //tryForAdAndRemove(driver);
            goToLogInPage(d,driver);

            try
            {
                IWebElement uName = driver.FindElement(By.XPath("//input[@type = 'email']"));
                IWebElement pWord = driver.FindElement(By.XPath("//input[@type = 'password']"));
                Console.WriteLine("Email: " + d.getEmail());
                Console.WriteLine("Password: " + d.getPassword());
                uName.SendKeys(d.getEmail());
                pWord.SendKeys(d.getPassword());
                pWord.Submit();
            }
            catch (Selenium.SeleniumException e)
            {
                Console.WriteLine("Selenium Error: " + e);
                return false;
            }
            //driver.Quit();
            Thread.Sleep(1000);
            return true;
        }

        public static bool tryForAdAndRemove(IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.CssSelector("[href*='#header']")).Click();
            }
            catch (Selenium.SeleniumException e)
            {
                Console.WriteLine("Selenium Error: " + e);
                return false;
            }
            return true;
        }


        private static bool goToLogInPage(Domain domain , IWebDriver driver)
        {
            try
            {
                IWebElement login = driver.FindElement(By.XPath(domain.loginButton));
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click()", login);
            }
            catch (Selenium.SeleniumException e)
            {
                Console.WriteLine("Selenium Error: " + e);
                return false;
            }
            return true;
        }
    }
}
