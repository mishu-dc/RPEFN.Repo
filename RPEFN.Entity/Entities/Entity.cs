using System;

namespace RPEFN.Data.Entities
{
    public class Entity
    {
        public Entity()
        {
            CreatedDate = DateTime.Now;
            CreatedBy = "Administrator";
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}