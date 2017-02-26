using System;

namespace RPEFN.WebService.Dtos
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string Dose { get; set; }
        public int Duration { get; set; }
        public DateTime WrittenDate { get; set; }
        public int PatientId { get; set; }
        public int DrugId { get; set; }
    }
}