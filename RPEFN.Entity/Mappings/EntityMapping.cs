using System.Data.Entity.ModelConfiguration;
using RPEFN.Data.Entities;

namespace RPEFN.Data.Mappings
{
    public class EntityMapping<TEntity>:EntityTypeConfiguration<TEntity> where TEntity:Entity
    {
        public EntityMapping()
        {
            HasKey(e => e.Id);
            Property(e => e.CreatedBy)
                .IsRequired();
            Property(e => e.CreatedDate)
                .IsRequired();
            Property(e => e.UpdatedBy)
                .IsOptional();
            Property(e => e.UpdatedDate)
                .IsOptional();
        }
    }
}