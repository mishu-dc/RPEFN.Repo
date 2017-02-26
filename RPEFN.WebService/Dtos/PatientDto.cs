using System;
using System.ComponentModel.DataAnnotations;

namespace RPEFN.WebService.Dtos
{
    public class PatientDto
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MaxLength(1)]
        [RegularExpression("^M$|^F$")]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}