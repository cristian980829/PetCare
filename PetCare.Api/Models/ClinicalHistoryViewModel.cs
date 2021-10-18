using System;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Api.Models
{
    public class ClinicalHistoryViewModel
    {
        public int PetId { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Remarks { get; set; }
    }
}
