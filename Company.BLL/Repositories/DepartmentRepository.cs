using Company.BLL.Interfaces;
using Company.DAL.Data.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class DepartmentRepository : GenaricRepository<Department>, IDepartmentRepository
    {
        private readonly CompanyDbContext _context;
        #region After
        //private readonly CompanyDbContext _context;
        //public DepartmentRepository(CompanyDbContext companyDbContext)
        //{
        //    _context = companyDbContext;
        //}
        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}

        //public Department? Get(int id)
        //{
        //    return _context.Departments.Find(id);
        //}

        //public int Add(Department department)
        //{
        //    _context.Departments.Add(department);
        //    return _context.SaveChanges();
        //}

        //public int Update(Department department)
        //{
        //    _context.Departments.Update(department);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _context.Departments.Remove(department);
        //    return _context.SaveChanges();
        //} 
        #endregion


        public DepartmentRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>>  GetDepartmentByNameAsync(string name)
        {
            return await _context.Departments
                .Where(e => e.Name.ToLower() == name.ToLower()).ToListAsync();
                //.Contains(name.ToLower())).ToList();
        }
    }
}
