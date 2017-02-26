using System.Collections.Generic;

namespace RPEFN.Data.Entities
{
    public class Drug : Entity
    {
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string NdcId { get; set; }
        public string Strength { get; set; }
        public List<Prescription> Prescriptions { get; set; }
        public double Price { get; set; }
    }
}