
namespace TaskManagementSystem.Core
{
    //Generic Interface implementing GET, GETALL, ADD, DELETE, UPDATE
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
