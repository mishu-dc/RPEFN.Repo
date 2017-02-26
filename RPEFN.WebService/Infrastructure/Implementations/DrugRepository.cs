using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPEFN.Data.Entities;
using RPEFN.WebService.Infrastructure.Interfaces;

namespace RPEFN.WebService.Infrastructure.Implementations
{
    public class DrugRepository:Repository<Drug>,IDrugRepository
    {
        public DrugRepository(DbContext context)
            : base(context)
        {
        }

        public Drug MostExpensiveDrug()
        {
            return Context.Set<Drug>().OrderByDescending(d => d.Price).Take(1).FirstOrDefault();
        }

        public async Task<Drug> MostExpensiveDrugAsync()
        {
            return await Context.Set<Drug>().OrderByDescending(d => d.Price).Take(1).FirstOrDefaultAsync();
        }
    }
}