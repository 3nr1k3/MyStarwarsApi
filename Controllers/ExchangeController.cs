
using System;
using Microsoft.AspNetCore.Mvc;

namespace MyStarwarsApi.Controllers{
    [Route("api/[controller]")]
    public class ExchangeController : Controller{

        [HttpGet]
        public String Get()
        {
            return "Hello World";
        }

        [HttpGet("{coin}/{cuantity}")]
        public String Get(String coin, float cuantity)
        {
            string result;

            if(coin == "euro")
                result = ((float)((int)cuantity * (float)166.386)).ToString("0");
            else
                result = ((float)cuantity / (float)166.386).ToString("0.00");

            var symbol = coin=="euro"? "pesetas" : "€" ;
            return $"{result} {symbol}";
        }
    }
}