using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class Employee: BaseEntity
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public bool IsACtive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HiringDate { get; set; }

        public string? ImageName { get; set; }
        // relation with department
        public int? DepartmentId { get; set; }

        // virtualليه ال 
        public virtual Department? Department { get; set; }
    }
}
