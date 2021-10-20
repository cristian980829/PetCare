using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Api.Models
{
    public class MedicalFormulaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Precio Medicamento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal MedicinePrice { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public int ClinicalHistoryId { get; set; }

        [Display(Name = "Medicamento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un medicamento.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int MedicineId { get; set; }

        public IEnumerable<SelectListItem> Medicines { get; set; }
    }
}
