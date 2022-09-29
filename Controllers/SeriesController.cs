using ApiSeries.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("api/series")]
    public class SeriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public SeriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet] // api/serie
        [HttpGet("listado")] // api/series/listado
        [HttpGet("/listado")] // /listado
        public async Task<ActionResult<List<Serie>>> Get()
        {
            return await dbContext.Series.Include(x => x.categorias).ToListAsync();
        }

        [HttpGet("primero")] //api/series/primero?
        public async Task<ActionResult<Serie>> PrimerSerie([FromHeader] int valor, [FromQuery] string serie, [FromQuery] int serieId)
        {
            return await dbContext.Series.FirstOrDefaultAsync();
        }

        [HttpGet("primero2")] //api/series/primero
        public ActionResult<Serie> PrimerSerieD()
        {
            return new Serie() { Name = "DOS"};
        }

        [HttpGet("{id:int}/{param=Peacemaker}")]
        public async Task<ActionResult<Serie>> Get(int id, string param)
        {
            var serie = await dbContext.Series.FirstOrDefaultAsync(x => x.Id == id);

            if(serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Serie>> Get([FromRoute] string nombre)
        {
            var serie = await dbContext.Series.FirstOrDefaultAsync(x => x.Name.Contains(nombre));

            if (serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Serie serie)
        {
            dbContext.Add(serie);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // api/serie/1
        public async Task<ActionResult> Put(Serie serie, int id)
        {
            var exist = await dbContext.Series.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

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
                return NotFound("El recurso no fue encontrado.");
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
