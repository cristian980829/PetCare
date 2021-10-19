using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetCare.Api.Data.Entities
{
    public class PetPhoto
    {
        public int Id { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet Pet { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Fix the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://petcareapi1camilocristian.azurewebsites.net/images/noimage.png"
            : $"https://petcarecamilocristian.blob.core.windows.net/petphotos/{ImageId}";
    }
}
