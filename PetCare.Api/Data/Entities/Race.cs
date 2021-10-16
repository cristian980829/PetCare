using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PetCare.Api.Data.Entities
{
    public class Race
    {
        public int Id { get; set; }

        [Display(Name = "Raza")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        public ICollection<Pet> pets { get; set; }
    }
}
