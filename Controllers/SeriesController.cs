using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSeries.Entidades;
using ApiSeries.Filtros;
using ApiSeries.Services;

namespace ApiSeries.Controllers
{
    [ApiController]
    [Route("api/series")]
    //[Authorize]
    public class SeriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<SeriesController> logger;
        private readonly IWebHostEnvironment env;
        private readonly string nuevosRegistros = "nuevosRegistros.txt";
        private readonly string registrosConsultados = "registrosConsultados.txt";


        public SeriesController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<SeriesController> logger)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
        }


        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        { 
            return Ok(new
            {
                SeriesControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                SeriesControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                SeriesControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet] // api/serie
        [HttpGet("listado")] // api/series/listado
        [HttpGet("/listado")] // /listado
        //[Authorize]

        //[ServiceFilter(typeof(FiltroDeAccion))]
        public async Task<ActionResult<List<Serie>>> GetSeries()
        {
            throw new NotImplementedException();
            logger.LogInformation("Se obtiene el listado de series");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
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


        [HttpGet("obtenerSerie/{nombre}")]
        public async Task<ActionResult<Serie>> Get([FromRoute] string nombre)
        {
            var serie = await dbContext.Series.FirstOrDefaultAsync(x => x.Name.Contains(nombre));

            if (serie == null)
            {
                logger.LogError("No se encuentra la serie. ");
                return NotFound();
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{registrosConsultados}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(serie.Id + " " + serie.Name); }

            return serie;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Serie serie)
        {
            var existeSerieMismoNombre = await dbContext.Series.AnyAsync(x => x.Name == serie.Name);

            if (existeSerieMismoNombre)
            {
                return BadRequest("Ya existe una serie con el nombre.");
            }

            
            dbContext.Add(serie);
            await dbContext.SaveChangesAsync();
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nuevosRegistros}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(serie.Id + " " + serie.Name); }

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
