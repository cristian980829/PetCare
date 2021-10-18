using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Api.Models
{
    public class PetViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de mascota")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de mascota.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PetTypeId { get; set; }

        public IEnumerable<SelectListItem> PetTypes { get; set; }

        [Display(Name = "Raza")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una raza.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int RaceId { get; set; }

        public IEnumerable<SelectListItem> Races { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Peso")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Weight { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

        public ICollection<PetPhoto> PetPhotos { get; set; }
    }
}
