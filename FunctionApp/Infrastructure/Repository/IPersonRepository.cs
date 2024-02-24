using FunctionApp.Domains.Bson;

namespace FunctionApp.Infrastructure.Repository
{
    public interface IPersonRepository
    {
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<PersonBson?> CreateAsync(PersonBson newPersonBson);
        Task<IEnumerable<PersonBson>> GetAllAsync(int currentPage, int perPage, CancellationToken cancellationToken);
        Task<IEnumerable<PersonBson>> GetAsync(CancellationToken cancellationToken);
        Task<PersonBson?> GetAsync(string id, CancellationToken cancellationToken);
        Task<bool> RemoveAsync(string id);
        Task<PersonBson?> UpdateAsync(string id, PersonBson updatedPersonBson);
    }
}