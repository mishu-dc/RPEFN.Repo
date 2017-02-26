using RPEFN.Data.Entities;

namespace RPEFN.Data.Mappings
{
    public class PatientMapping:EntityMapping<Patient>
    {
        public PatientMapping()
        {
            Property(p => p.DateOfBirth)
                .IsRequired();

            Property(p => p.FirstName)
                .HasMaxLength(255)
                .IsRequired();

            Property(p => p.LastName)
                .HasMaxLength(255)
                .IsRequired();

            Property(p => p.DateOfBirth)
                .IsRequired();


            Property(p => p.Gender)
                .HasMaxLength(1)
                .IsRequired();

            HasMany(p => p.Prescriptions)
                .WithRequired(p => p.Patient)
                .HasForeignKey(p => p.PatientId);

        }
    }
}