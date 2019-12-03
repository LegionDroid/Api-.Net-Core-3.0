using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapiEf.Data;
using webapiEf.Models;

namespace webapiEf.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        
        [HttpGet]    //se não adicionar o verbo, ele fica get por padrão
        [Route("")] //concatena com oq esta na rota da classe //rota vazia, fica definido como a rota da classe
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post(
            [FromServices] DataContext context,
            [FromBody] Category model)
            {
                if (ModelState.IsValid)
                {
                    context.Categories.Add(model);
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