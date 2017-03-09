using System.ComponentModel.DataAnnotations;

namespace RPEFN.WebService.Dtos
{
    public class DrugDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        public string BrandName { get; set; }
        [Required]
        [MinLength(1)]
        public string GenericName { get; set; }
        [Required]
        public string NdcId { get; set; }
        public string Strength { get; set; }
        [Required]
        public double Price { get; set; }
    }
}