using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI3_1.Filters;

namespace WebAPI3_1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [AddHeaderAttribute("Name", "Vivek Kumar Tiwary")]
        [WeatherActionFilterAttribute("X_CustomHeader", "CutomHeader value")]
        [HttpGet]
        
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            var dockerImageId = Environment.MachineName;  
            var header = HttpContext.Request.Headers.TryGetValue("X_CUSTOM", out var X_CustomHeader);
            _logger.LogInformation($"header value from action filter attribute {X_CustomHeader}");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                ExtraInfo = $"Docker Image Id is : {Environment.MachineName}"
            })
            .ToArray();
            //throw new Exception("My new execption");
        }
    }
}
