using ApiSeries.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSeries.Controllers
{

    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriasController (ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAll()
        {
            return await dbContext.Categorias.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            return await dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Categoria categorias)
        {
            var existeSerie = await dbContext.Series.AnyAsync(x => x.Id == categorias.SerieId);

            if(!existeSerie)
            {
                return BadRequest($"No existe la serie con el id: {categorias.SerieId}");
            }

            dbContext.Add(categorias);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]   
        public async Task<ActionResult> Put(Categoria categorias, int id)
        {
            var exist = await dbContext.Categorias.AnyAsync (x => x.Id == id);

            if(!exist)
            {
                return NotFound("La categoria especificada no existe.");
            }

            if (categorias.Id != id)
            {
                return BadRequest("El id de la categoria no coincide con el establecido en la url.");
            }

            dbContext.Update(categorias);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Categorias.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound("El Recurso no fue encontrado");
            }

            dbContext.Remove(new Categoria { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
