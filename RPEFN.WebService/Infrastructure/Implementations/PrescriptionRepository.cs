using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPEFN.Data.Entities;
using RPEFN.WebService.Infrastructure.Interfaces;

namespace RPEFN.WebService.Infrastructure.Implementations
{
    public class PrescriptionRepository:Repository<Prescription>,IPrescriptionRepository
    {
        public PrescriptionRepository(DbContext context) : 
            base(context)
        {
        }

        public IEnumerable<Prescription> GetAllPrescriptionsByPatient(int patientId)
        {
            return Context.Set<Prescription>().Where(p => p.Patient.Id == patientId).ToList();
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsByPatientAsync(int patientId)
        {
            return await Context.Set<Prescription>().Where(p => p.Patient.Id == patientId).ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsWithDrugAndPatientAsync()
        {
            return await Context.Set<Prescription>().Include(p => p.Patient).Include(p => p.Drug).ToListAsync();
        }
    }
}