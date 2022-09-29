using System.ComponentModel.DataAnnotations;

namespace ApiSeries.Entidades
{
    public class Categoria
    {

        public int Id { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        public string Name { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        public  string Genero { get; set; }
        [Range(1, 20, ErrorMessage = "El campo {0} no se encuentra dentro del rango.")]
        public int SerieId { get; set; }

        public Serie Serie { get; set; }
    }
}
