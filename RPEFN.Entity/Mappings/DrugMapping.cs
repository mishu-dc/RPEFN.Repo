using RPEFN.Data.Entities;

namespace RPEFN.Data.Mappings
{
    public class DrugMapping:EntityMapping<Drug>
    {
        public DrugMapping()
        {
            ToTable("Drugs");
            Property(d => d.BrandName)
                .IsRequired()
                .HasMaxLength(255);

            Property(d => d.GenericName)
                .IsRequired()
                .HasMaxLength(255);

            Property(d => d.NdcId)
                .IsRequired()
                .HasMaxLength(11);

            Property(d => d.Strength)
                .IsRequired()
                .HasMaxLength(100);

            Property(d => d.Price)
                .IsRequired();

            HasMany(d=>d.Prescriptions)
                .WithRequired(p=>p.Drug)
                .WillCascadeOnDelete(true);

        }
    }
}