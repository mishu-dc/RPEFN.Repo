using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RPEFN.Data.Entities;
using RPEFN.WebService.Infrastructure.Interfaces;

namespace RPEFN.WebService.Infrastructure.Implementations
{
    public class PatientRepository:Repository<Patient>,IPatientRepository
    {
        public PatientRepository(DbContext context)
            :base(context)
        {
                
        }

        public int PatientCount(Expression<Func<Patient, bool>> predicate)
        {
            return Context.Set<Patient>().Where(predicate).Count();
        }

        public async Task<int> PatientCountAsync(Expression<Func<Patient, bool>> predicate)
        {
            return await Context.Set<Patient>().Where(predicate).CountAsync();
        }
    }
}