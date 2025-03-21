using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;

namespace Company.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRepository employeeRepository { get; }

        public IDepartmentRepository departmentRepository { get; }
        public CompanyDbContext _context { get; }

        public UnitOfWork(CompanyDbContext Context)
        {
            _context = Context;
            departmentRepository = new DepartmentRepository(_context);
            employeeRepository = new EmployeeRepository(_context);

        }

        public async Task<int> completeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}
    }
}
