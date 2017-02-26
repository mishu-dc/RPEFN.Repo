using System.Threading.Tasks;
using RPEFN.Data.Entities;

namespace RPEFN.WebService.Infrastructure.Interfaces
{
    public interface IDrugRepository:IRepository<Drug>
    {
        Drug MostExpensiveDrug();
        Task<Drug> MostExpensiveDrugAsync();
    }
}