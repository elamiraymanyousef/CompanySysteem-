using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IDepartmentRepository: IGenaricRepository<Department>
    {
        Task<IEnumerable<Department>> GetDepartmentByNameAsync(string name);
        //IEnumerable<Department> GetAll();
        //Department? Get(int id);
        //int Add(Department department);
        //int Update(Department department);
        //int Delete(Department department);
    }
}
