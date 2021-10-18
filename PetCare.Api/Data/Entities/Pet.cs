using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PetCare.Api.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public PetType PetType { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Raza")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Race Race { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Peso")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Weight { get; set; }

        [Display(Name = "Propietario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public ICollection<PetPhoto> PetPhotos { get; set; }

        [Display(Name = "# Fotos")]
        public int PetPhotosCount => PetPhotos == null ? 0 : PetPhotos.Count;

        //TODO: Fix the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => PetPhotos == null || PetPhotos.Count == 0
            ? $"https://localhost:44334/images/noimage.png"
            : PetPhotos.FirstOrDefault().ImageFullPath;

        public ICollection<ClinicalHistory> ClinicalHistories { get; set; }

        [Display(Name = "# Historias")]
        public int ClinicalHistoriesCount => ClinicalHistories == null ? 0 : ClinicalHistories.Count;
    }
}
