using ABE.LTRIC.Core.Data;
using ABE.LTRIC.Core.Interfaces;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private LTRIC_Context _context { get; set; }
        public Repository(LTRIC_Context context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAll()
        {
            return await Task.FromResult(_context.Set<T>().ToList());
        }

        public async Task Update(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IList> Get(ISpecification<T> specification)
        {
            return (await Task.FromResult(SpecificationEvaluator.Default.GetQuery(_context.Set<T>().AsQueryable(), specification))).ToList();
        }

        public async Task SaveAll(IList<T> entites)
        {
            foreach(var entity in entites)
            {
                if(entity.Id==0)
                {
                    _context.Set<T>().Add(entity);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
