using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Domain.Interfaces.Services
{

    public interface IBaseDomainService<TEntity, TKey> : IDisposable
        where TEntity : class
    {
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);

        Task<List<TEntity>>? GetAll();
        Task<TEntity>? GetById(TKey id);
    }
}
