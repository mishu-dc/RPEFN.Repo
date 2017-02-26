using RPEFN.Data.Entities;

namespace RPEFN.Data.Mappings
{
    public class PrescriptionMapping:EntityMapping<Prescription>
    {
        public PrescriptionMapping()
        {
            Property(p => p.Dose)
                .IsRequired()
                .HasMaxLength(255);

            Property(p => p.Duration)
                .IsRequired();

            Property(p => p.WrittenDate)
                .IsRequired();

            HasRequired(p => p.Drug)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(d => d.DrugId);

        }
    }
}