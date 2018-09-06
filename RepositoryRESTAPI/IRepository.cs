using System.Net;
using System.Threading.Tasks;

namespace RepositoryRESTAPI
{
    public interface IRepository
    {
        Task<T> AddAsync<T>(T entity, string requestUri);

        Task<HttpStatusCode> DeleteAsync(string requestUri);

        Task EditAsync<T>(T t, string requestUri);

        Task<T> GetAsync<T>(string path);
    }
}
