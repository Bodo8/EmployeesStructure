using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesStructure.Models
{
    public class Vacation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateSience { get; set; }
        public DateTime DateUntil { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }  
    }
}