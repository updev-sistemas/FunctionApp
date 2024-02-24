
using FunctionApp.Domains.Bson;
using HotChocolate.Data;
using MongoDB.Driver;

namespace FunctionApp.Infrastructure.Query;

public class PersonQuery
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PersonBson> GetPeople([Service] IMongoCollection<PersonBson> collection) =>
        collection.AsQueryable();
}
