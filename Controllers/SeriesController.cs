using ApiSeries.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("series")]
    public class SeriesController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public SeriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Serie>>> Get()
        {
            return await dbContext.Series.Include(x => x.categorias).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Serie serie)
        {
            dbContext.Add(serie);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Serie serie, int id)
        {
            if (serie.Id != id)
            {
                return BadRequest("El id de la serie no coincide con el establecido en la url.");
            }

            dbContext.Update(serie);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Series.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Serie()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
