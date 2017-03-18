using System;
using System.ComponentModel.DataAnnotations;

namespace RPEFN.WebService.Dtos
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string Dose { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public DateTime WrittenDate { get; set; }
        public PatientDto Patient { get; set; }
        public DrugDto Drug { get; set; }
        public int DrugId { get; set; }
        public int PatientId { get; set; }
    }
}