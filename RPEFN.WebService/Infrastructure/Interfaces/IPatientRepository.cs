using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RPEFN.Data.Entities;

namespace RPEFN.WebService.Infrastructure.Interfaces
{
    public interface IPatientRepository:IRepository<Patient>
    {
        int PatientCount(Expression<Func<Patient, bool>> predicate);
        Task<int> PatientCountAsync(Expression<Func<Patient, bool>> predicate);
    }
}
