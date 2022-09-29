using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSeries.Entidades
{
    public class Serie
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength:20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        public string Name { get; set; }
        [Range(1,20, ErrorMessage = "El campo {0} no se encuentra dentro del rango.")]
        [NotMapped]
        public int Temporadas { get; set; }

        public List<Categoria> categorias { get; set; }
    }
}
