using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesStructure.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int? SuperiorId { get; set; }

        [ForeignKey("SuperiorId")]
        public virtual Employee Superior { get; set; }

        public virtual ICollection<Employee> Subordinates { get; set; }

        public Employee()
        {
            Subordinates = new List<Employee>();
        }

        public ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int VacationPackageId { get; set; }
        public virtual VacationPackage VacationPackage { get; set; }
    }
}