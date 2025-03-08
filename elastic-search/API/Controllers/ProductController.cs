using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ProductService service) : ControllerBase
    {
        [HttpPost("add-products")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }
            if (string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }
            await service.IndexProductAsync(product);

            return Ok("Product added successfully.");
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await service.DeleteProduct(id);
            if (result)
            {
                return Ok("Product deleted successfully.");
            }
            return BadRequest("Product did not exist or failed to delete product.");
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await service.GetProducts();
            return Ok(products);
        }

        [HttpGet("search-price-range")]
        public async Task<IActionResult> SearchProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var products = await service.SearchProductsByPriceRange(minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("search-wildcard/{searchTerm}")]
        public async Task<IActionResult> SearchProductsWithWildCard(string searchTerm)
        {
            var products = await service.SearchProductWithWildCard(searchTerm);
            return Ok(products);
        }

        [HttpGet("search-fuzzy/{searchTerm}")]
        public async Task<IActionResult> SearchProductsWithFuzzy(string searchTerm)
        {
            var products = await service.SearchProductWithFuzzy(searchTerm);
            return Ok(products);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            var products = await service.SearchProducts(searchTerm);
            return Ok(products);
        }
    }
}