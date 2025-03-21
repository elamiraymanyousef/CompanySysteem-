using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class Department:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }

        // navigation property
        public virtual ICollection<Employee> Employees { get; set; }=new HashSet<Employee>();

    }
}
