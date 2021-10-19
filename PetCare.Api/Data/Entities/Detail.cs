using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetCare.Api.Data.Entities
{
    public class Detail
    {
        public int Id { get; set; }

        [Display(Name = "Historia Clínica")]
        [JsonIgnore]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ClinicalHistory ClinicalHistory { get; set; }

        [Display(Name = "Procedimiento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Procedure Procedure { get; set; }

        [Display(Name = "Precio del procedimiento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal ProcedurePrice { get; set; }

        [Display(Name = "Precio Medicina")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal MedicinePrice { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalPrice => ProcedurePrice + MedicinePrice ;

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }
}
