using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI3_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
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
                var client = _clientFactory.CreateClient("animal");
                var animals = await client.GetStringAsync($"entries?category=animals&https=true");
                var ouput = JsonConvert.DeserializeObject(animals);
                return Ok(ouput);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}