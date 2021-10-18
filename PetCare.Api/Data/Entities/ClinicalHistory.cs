using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace PetCare.Api.Data.Entities
{
    public class ClinicalHistory
    {
        public int Id { get; set; }

        [Display(Name = "Mascota")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Pet Pet { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        [Display(Name = "Observación")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [JsonIgnore]
        [Display(Name = "Veterinario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public User User { get; set; }

        public ICollection<Detail> Details { get; set; }

        [Display(Name = "# Detalles")]
        public int DetailsCount => Details == null ? 0 : Details.Count;

        [Display(Name = "Total Procedimiento")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalProcedure => Details == null ? 0 : Details.Sum(x => x.ProcedurePrice);

        [Display(Name = "Total Medicina")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalMedicine => Details == null ? 0 : Details.Sum(x => x.MedicinePrice);

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Total => Details == null ? 0 : Details.Sum(x => x.TotalPrice);
    }
}
