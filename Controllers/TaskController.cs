using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebAPI3_1.Services.Factory;
using WebAPI3_1.Services.Implementations;

namespace WebAPI3_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {

        //using client factory will use the underlying httphandlers which has been already established.
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<TaskController> _logger;

        public TaskController(IHttpClientFactory clientFactory, ILogger<TaskController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [HttpGet("{accountName}")]
        public async Task<IActionResult> GetUserInfo(string accountName)
        {
            try
            {
                var client = _clientFactory.CreateClient("github");
                var githubAccount = await client.GetStringAsync($"{accountName}");
                var ouput = JsonConvert.DeserializeObject(githubAccount);
                return Ok(ouput);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


//factory pattern

        [HttpGet("getOrderCost/{ordCode}")]
        public double CalculateCost(string ordCode)
        {
            var calculator = OrderFactory.GetOrderCalculator(ordCode);
            return calculator.CalculateOrder(ordCode);
        }
    }
}