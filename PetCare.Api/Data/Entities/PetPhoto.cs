using System;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Api.Data.Entities
{
    public class PetPhoto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet Pet { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Fix the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44345/images/noimage.png"
            : $"https://vehicleszuluprep.blob.core.windows.net/vehicles/{ImageId}";
    }
}
