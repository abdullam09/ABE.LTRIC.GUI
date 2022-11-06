using ABE.LTRIC.Core.Data;
using ABE.LTRIC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Task.FromResult(_context.Set<T>().AsEnumerable());
        }

        public async Task Update(T entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
