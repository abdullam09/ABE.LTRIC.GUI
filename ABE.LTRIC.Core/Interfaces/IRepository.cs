using ABE.LTRIC.Core.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABE.LTRIC.Core.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAll();
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
