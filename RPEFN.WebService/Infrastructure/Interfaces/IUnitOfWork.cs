using System;
using System.Threading.Tasks;

namespace RPEFN.WebService.Infrastructure.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        int Complete();
        Task<int> CompleteAsync();
        IPatientRepository Patients { get; }
        IDrugRepository Drugs { get; }
        IPrescriptionRepository Prescriptions { get; }
    }
}
