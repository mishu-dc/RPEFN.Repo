using System;
using System.Collections.Generic;

namespace RPEFN.Data.Entities
{
    public class Patient: Data.Entities.Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<Prescription> Prescriptions { get; set; }
    }
}