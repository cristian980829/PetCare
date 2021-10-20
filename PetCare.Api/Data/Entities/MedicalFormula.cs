using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetCare.Api.Data.Entities
{
    public class MedicalFormula
    {
        public int Id { get; set; }

        [Display(Name = "Historia Clínica")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ClinicalHistory ClinicalHistory { get; set; }

        [Display(Name = "Medicamento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Medicine Medicine { get; set; }

        [Display(Name = "Precio Medicamento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal MedicinePrice { get; set; }

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }
}
