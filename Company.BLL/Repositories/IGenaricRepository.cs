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
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _companyDb;

        public GenaricRepository(CompanyDbContext companyDb)
        {
            _companyDb = companyDb;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // to include department name in the employee list
            // return _companyDb.Set<T>().Include("Department").ToList();
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _companyDb.Set<T>().Cast<Employee>().Include(e => e.Department).ToListAsync();
            }
            return await _companyDb.Set<T>().ToListAsync();
        }


        public async Task<T?> GetAsync(int id)
        {
            return await _companyDb.Set<T>().FindAsync(id);
        }


        public async Task AddAsync(T department)
        {
            await _companyDb.Set<T>().AddAsync(department);
            //return _companyDb.SaveChanges();
        }


        public void Update(T department)
        {
            _companyDb.Set<T>().Update(department);
            //return _companyDb.SaveChanges();
        }


        public void Delete(T department)
        {
            _companyDb.Set<T>().Remove(department);
            //return _companyDb.SaveChanges();
        }
    }
    }
