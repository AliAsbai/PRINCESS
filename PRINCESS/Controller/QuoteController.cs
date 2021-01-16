using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PRINCESS.model;

/**
 *  authors:
 *          @Ali Asbai
 *          @Gabriel Vega
 *          
 **/

namespace PRINCESS.controller
{
    public class QuoteFormat
    {
        public string Quote { get; set; }
        public Review Review { get; set; }

        public string User { get; set;  }
    }

    [Route("[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        [HttpPost("activeQuote")]
        public void Post(List<QuoteFormat> Quotes)
        {
            Task insert = new Task(() => new Action<List<QuoteFormat>>(insertQuoteDatabase)(Quotes));

            insert.Start();
            insert.Wait();
            Task.WaitAll();
        }

        public static void insertQuoteDatabase(List<QuoteFormat> Quotes)
        {
            foreach(QuoteFormat q in Quotes)
            {
                q.User = "test@email.com";
                PrincessDb.insertQuote(q);
            }
        }
    }
}
