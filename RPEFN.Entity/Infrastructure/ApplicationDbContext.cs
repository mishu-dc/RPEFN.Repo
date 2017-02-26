using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using RPEFN.Data.Entities;
using RPEFN.Data.Mappings;

namespace RPEFN.Data.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("RPEFN", throwIfV1Schema: false)
        {
        }

        private DbSet<Patient> Patients { get; set; }
        private DbSet<Drug> Drugs { get; set; }
        private DbSet<Prescription> Prescriptions { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new PatientMapping());
            modelBuilder.Configurations.Add(new DrugMapping());
            modelBuilder.Configurations.Add(new PrescriptionMapping());
            
        }
    }
}