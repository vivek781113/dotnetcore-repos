using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI3_1.Models;
using WebAPI3_1.Utils;

namespace WebAPI3_1.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("[controller]/[action]")]
    [Route("v{v:apiVersion}/products/[action]")]
    public class ProductsV1_0Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;
        private readonly ILogger<ProductsV1_0Controller> _logger;

        public ProductsV1_0Controller(ShopContext shopContext, ILogger<ProductsV1_0Controller> logger)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
            _logger = logger;
        }

        public void TraceMessage(string message,
                [CallerMemberName] string memberName = "",
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("message: " + message);
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file path: " + sourceFilePath);
            Trace.WriteLine("source line number: " + sourceLineNumber);
            _logger.LogInformation($"Method: {memberName} \nMessage: {message}");
        }
        [HttpGet]
        public IActionResult GetProducts([FromQuery] QueryParameters queryParameters)
        {
            TraceMessage("Init method call");
            IQueryable<Product> products = _shopContext.Products;
            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(products.ToArray());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(_shopContext.Products.FirstOrDefault(p => p.Id == id));

        [HttpPost]
        public IActionResult NewProduct([FromBody] Product product)
        {
            _shopContext.Products.Add(product);
            _shopContext.SaveChanges();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var productToUpdate = _shopContext.Products.FirstOrDefault(p => p.Id == id);

            if (productToUpdate == null)
                return NotFound();

            patchDoc.ApplyTo(productToUpdate, ModelState);

            bool isValid = TryValidateModel(productToUpdate);

            if (!isValid)
                return BadRequest(ModelState);

            _shopContext.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = _shopContext.Products.FirstOrDefault(p => p.Id == id);

            if (productToDelete == null)
                return NotFound();

            _shopContext.Remove(productToDelete);
            int result = _shopContext.SaveChanges();

            return NoContent();
        }

    }

    // version two of Prdoucts controller
    //here we are reading version from header


    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    //[Route("[controller]/[action]")]
    [Route("products/[action]")]
    //[Route("v{v:apiVersion}/products/[action]")]
    public class ProductsV2_0Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;
        private readonly ILogger<ProductsV2_0Controller> _logger;

        public ProductsV2_0Controller(ShopContext shopContext, ILogger<ProductsV2_0Controller> logger)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
            _logger = logger;
        }

        public void TraceMessage(string message,
                [CallerMemberName] string memberName = "",
                [CallerFilePath] string sourceFilePath = "",
                [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("message: " + message);
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file path: " + sourceFilePath);
            Trace.WriteLine("source line number: " + sourceLineNumber);
            _logger.LogInformation($"Method: {memberName} \nMessage: {message}");
        }
        [HttpGet]
        public IActionResult GetProducts([FromQuery] QueryParameters queryParameters)
        {
            TraceMessage("Init method call");

            IQueryable<Product> products = _shopContext.Products.Include(p => p.Category);

            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(products.ToArray());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(_shopContext.Products.FirstOrDefault(p => p.Id == id));

        [HttpPost]
        public IActionResult NewProduct([FromBody] Product product)
        {
            _shopContext.Products.Add(product);
            _shopContext.SaveChanges();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var productToUpdate = _shopContext.Products.FirstOrDefault(p => p.Id == id);

            if (productToUpdate == null)
                return NotFound();

            patchDoc.ApplyTo(productToUpdate, ModelState);

            bool isValid = TryValidateModel(productToUpdate);

            if (!isValid)
                return BadRequest(ModelState);

            _shopContext.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = _shopContext.Products.FirstOrDefault(p => p.Id == id);

            if (productToDelete == null)
                return NotFound();

            _shopContext.Remove(productToDelete);
            int result = _shopContext.SaveChanges();

            return NoContent();
        }

    }
}
