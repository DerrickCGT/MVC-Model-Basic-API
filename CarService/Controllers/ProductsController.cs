using CarService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ShopContext _shopContext;


        public ProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductParametersQuery queryParameters)
        {
            //return Ok(await _shopContext.Products.ToArrayAsync());
            IQueryable<Product> products = _shopContext.Products;

            if (queryParameters.MinPrice != null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.MinPrice.Value);
            }
            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price <= queryParameters.MaxPrice.Value);
            }
            //if (!string.IsNullOrEmpty(queryParameters.Sku))
            //{
            //    products = products.Where(
            //        p => p.Sku == queryParameters.Sku);
            //}
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(
                   p => p.Name.ToLower().Contains(
                       queryParameters.Name.ToLower()));
            }

            int skipCount = (queryParameters.Size * (queryParameters.Page - 1)); // include pagination 
            products = products.Skip(skipCount).Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());

        }

        [Route("api/[controller]")]

        [HttpGet]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            // here we use Ok product
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        //[HttpGet]
        //public ActionResult<IEnumerable<Product>> GetAllProducts()
        //{
        //    var products = _shopContext.Products.ToArray();
        //    return Ok(products);
        //}

        //[Route("ID")]

        //[HttpGet]

        //public ActionResult GetProduct(int id)
        //{
        //    var products = _shopContext.Products.Find(id);
        //    return Ok(products);
        //}

        //[Route("name")]
        //[HttpGet]

        //public ActionResult GetProductName(int id)
        //{
        //    var products = _shopContext.Products.Find(id);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(products.Name);
        //}

        //[HttpGet, Route("/products/all")]

        //public async Task<IEnumerable<Product>> GetAllProducts2()
        //{
        //    return await _shopContext.Products.ToArrayAsync();
        //}

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                    product);
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_shopContext.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null) return NotFound();

            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();

            return product;
        }

    }
}
