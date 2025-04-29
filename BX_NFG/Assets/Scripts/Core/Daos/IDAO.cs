using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assets.Scripts.Core.Daos
{
    public interface IDAO<T, ID>
    {
        public Task<T> select(ID id);
        public Task<List<T>> selectAll();
        public Task<bool> insert(T entity);
        public Task<bool> update(T entity);
        public Task<bool> delete(ID id);
    }
}
