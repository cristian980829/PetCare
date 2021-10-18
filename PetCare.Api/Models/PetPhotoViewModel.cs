using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Api.Models
{
    public class PetPhotoViewModel
    {
        public int PetId { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
    }
}
