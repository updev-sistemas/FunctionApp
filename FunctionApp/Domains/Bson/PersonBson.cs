using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FunctionApp.Domains.Bson;

public class PersonBson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("id")]
    public virtual string? Id { get; set; }

    [BsonElement("name")]
    public virtual string? Name { get; set; }

    [BsonElement("email")]
    public virtual string? Email { get; set; }

    [BsonElement("birthday")]
    public virtual DateOnly? Birthday { get; set; }

    [BsonElement("created_at")]
    public virtual DateTime? CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public virtual DateTime? UpdatedAt { get; set; }
}
