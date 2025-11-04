using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesStructure.Models
{
    public class Calendar
    {
        [Key]
        public DateTime Date { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
    }
}