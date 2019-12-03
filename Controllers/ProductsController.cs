using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapiEf.Data;
using webapiEf.Models;

namespace webapiEf.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context){

            var products = await context.Products.Include(x => x.Category).ToListAsync();
            return products;
            
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product  = await context.Products.Include(x => x.Category)
            .AsNoTracking() //não cria proxy dos objetos
            .FirstOrDefaultAsync(x => x.Id == id); //FirstOrDefaultAsync -> Traz o primeiro se encontrar mais de um, se encontrar só um item traz esse item, e se não achar nenhum item retorna nulo, e não da nenhum erro.
            return product;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products  = await context.Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoryId == id)
            .ToListAsync(); 
            return products;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model)
        {
            if(ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}   