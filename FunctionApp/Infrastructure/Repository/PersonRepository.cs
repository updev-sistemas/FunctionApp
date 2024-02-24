using FunctionApp.Domains.Bson;
using MongoDB.Driver;
using System.Threading;

namespace FunctionApp.Infrastructure.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly IMongoCollection<PersonBson> _PersonBsonCollection;

    public PersonRepository()
    {
        var mongoClient = new MongoClient("");

        var mongoDatabase = mongoClient.GetDatabase("person-sa");

        _PersonBsonCollection = mongoDatabase.GetCollection<PersonBson>("people");
    }


    public async Task<IEnumerable<PersonBson>> GetAsync(CancellationToken cancellationToken) =>
        await _PersonBsonCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<PersonBson?> GetAsync(string id, CancellationToken cancellationToken) =>
        await _PersonBsonCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task<PersonBson?> CreateAsync(PersonBson newPersonBson)
    {
        await _PersonBsonCollection.InsertOneAsync(newPersonBson);

        return newPersonBson;
    }

    public async Task<PersonBson?> UpdateAsync(string id, PersonBson updatedPersonBson)
    {
        var updatedPerson = await GetAsync(id, CancellationToken.None);

        if (updatedPerson is null)
        {
            return default;
        }

        updatedPerson.Name = updatedPersonBson.Name ?? updatedPerson.Name;
        updatedPerson.Email = updatedPersonBson.Email ?? updatedPerson.Email;
        updatedPerson.Birthday = updatedPersonBson.Birthday ?? updatedPerson.Birthday;

        var result = await _PersonBsonCollection.ReplaceOneAsync(x => x.Id == id, updatedPerson);

        if (result.IsModifiedCountAvailable && result.ModifiedCount == 0)
        {
            return default;
        }

        return updatedPersonBson;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var result = await _PersonBsonCollection.DeleteOneAsync(x => x.Id == id);

        return (result.DeletedCount > 0);
    }

    public async Task<IEnumerable<PersonBson>> GetAllAsync(int currentPage, int perPage, CancellationToken cancellationToken)
        => await _PersonBsonCollection.Find(_ => true)
                                      .Skip((perPage - 1) * currentPage)
                                      .Limit(currentPage)
                                      .ToListAsync(cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        var count = await _PersonBsonCollection.CountDocumentsAsync(FilterDefinition<PersonBson>.Empty, cancellationToken: cancellationToken);
        return (int)count;
    }
}
