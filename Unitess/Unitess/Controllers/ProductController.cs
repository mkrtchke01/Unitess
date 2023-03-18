using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unitess.Models;
using Unitess.Requests;
using Unitess.Services.Interfaces;

namespace Unitess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("/GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProducts getAllProducts)
        {
            var products = await _productService.GetAllAsync(getAllProducts);
            return Ok(products);
        }
        [HttpGet]
        [Route("/Get/{productId}")]
        public async Task<IActionResult> Get([FromRoute] int productId)
        {
            var product = await _productService.GetAsync(productId);
            return Ok(product);
        }
        [HttpPost]
        [Route("/Create")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest createProductRequest)
        {
            await _productService.CreateAsync(createProductRequest);
            return Ok(createProductRequest);
        }
        [HttpPost]
        [Route("/Update/{productId}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest updateProductRequest, [FromRoute] int productId)
        {
            await _productService.UpdateAsync(updateProductRequest, productId);
            return Ok();
        }
        [HttpPost]
        [Route("/Delete/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            await _productService.DeleteAsync(productId);
            return Ok();
        }
    }
}
