using System.Threading.Tasks;
using RPEFN.Data.Infrastructure;
using RPEFN.WebService.Infrastructure.Interfaces;

namespace RPEFN.WebService.Infrastructure.Implementations
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(context);
            Drugs = new DrugRepository(context);
            Prescriptions = new PrescriptionRepository(context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IPatientRepository Patients { get; }
        public IDrugRepository Drugs { get; }
        public IPrescriptionRepository Prescriptions { get; }
    }
}