using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRINCESS.model;
using PRINCESS.model.screenScraper;

/** 
 * authors:
 *          @Anna Mann
 *          @Gabriel Vega
 *          @Olivia Höft
 *          
 **/


namespace PRINCESS.controller
{
    [Route("[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        [Authorize]
        [HttpPost("Domain")]
        public void Post([FromBody] DomainFormat model)
        {
            Domain d = new Domain();
            d.setTitle(new model.screenScraper.HtmlElement(model.Title));

            d.setWriter(new model.screenScraper.HtmlElement(model.Writer));

            d.setReview(new model.screenScraper.HtmlElement(model.Review));

            d.setMenu(new model.screenScraper.HtmlElement(model.Menu));
            d.setNextButton(model.NbValue);

            d.setCountry(model.Country);
            d.setPriority(model.Priority);
            d.setURL(model.URL);
            d.setRating(new model.screenScraper.HtmlElement(model.Rating));

            d.setEmail(model.Login);
            d.setPassword(model.Password);

            Task insert = new Task(() => new Action<Domain>(insertDomainDb)(d));

            insert.Start();
            insert.Wait();
            Task.WaitAll();
        }

        public static void insertDomainDb(Domain domain)
        {
            PrincessDb.insertDomain(domain);
        }
    }
}
