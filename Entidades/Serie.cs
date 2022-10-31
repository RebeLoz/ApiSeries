using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiSeries.Validaciones;

namespace ApiSeries.Entidades
{
    public class Serie : IValidatableObject
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength:20, ErrorMessage = "El campo {0} solo puede tener hasta 20 caracteres.")]
        //[PrimerLetraMayuscula]
        public string Name { get; set; }

        [Range(1,20, ErrorMessage = "El campo {0} no se encuentra dentro del rango.")]
        [NotMapped]
        public int Temporadas { get; set; }

        public List<Categoria> categorias { get; set; }

        [NotMapped]
        public int Menor { get; set; }

        [NotMapped]
        public int Mayor { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var primeraLetra = Name[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Name) });
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser más grande que el campo Mayor",
                    new String[] { nameof(Menor) });
            }
        }
    }
}
