using System;

namespace RPEFN.Data.Entities
{
    public class Prescription : Data.Entities.Entity
    {
        public string Dose { get; set; }
        public int Duration { get; set; }
        public DateTime WrittenDate { get; set; }
        public Drug Drug { get; set; }
        public Patient Patient { get; set; }
        public int PatientId { get; set; }
        public int DrugId { get; set; }
    }
}