using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository :GenaricRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;
        #region After
        //private readonly CompanyDbContext _companyDb;

        //public EmployeeRepository(CompanyDbContext companyDb)
        //{
        //    _companyDb = companyDb;
        //}
        //public IEnumerable<Employee> GetAll()
        //{
        //    return _companyDb.Employees.ToList();
        //}

        //public Employee? Get(int id)
        //{
        //    return _companyDb.Employees.Find(id);
        //}

        //public int Update(Employee department)
        //{
        //    _companyDb.Employees.Update(department);
        //    return _companyDb.SaveChanges();
        //}

        //public int Add(Employee department)
        //{
        //    _companyDb.Employees.Add(department);
        //    return _companyDb.SaveChanges();    
        //}

        //public int Delete(Employee department)
        //{
        //    _companyDb.Employees.Remove(department);
        //    return _companyDb.SaveChanges();
        //} 
        #endregion

        // ask Clr to call the base class constructor
        // to pass the context to the base class GenaricRepository
        public EmployeeRepository(CompanyDbContext context) :base(context) 
        {
            _context = context;
        }

        public  async Task<IEnumerable<Employee>>  GetByNameAsync(string SearchName)
        {
            return await _context.Employees.Include(D => D.Department)
                .Where(e => e.Name.ToLower()
                .Contains(SearchName.ToLower())).ToListAsync();
        }
    }
}
