using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesStructure.Models
{
    public class VacationPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int GrantedDays { get; set; }

        public int Year { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}