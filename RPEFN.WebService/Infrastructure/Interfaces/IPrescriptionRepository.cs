using System.Collections.Generic;
using System.Threading.Tasks;
using RPEFN.Data.Entities;

namespace RPEFN.WebService.Infrastructure.Interfaces
{
    public interface IPrescriptionRepository:IRepository<Prescription>
    {
        IEnumerable<Prescription> GetAllPrescriptionsByPatient(int patientId);
        Task<IEnumerable<Prescription>> GetAllPrescriptionsByPatientAsync(int patientId);
        Task<IEnumerable<Prescription>> GetAllPrescriptionsWithDrugAndPatientAsync();
    }
}
