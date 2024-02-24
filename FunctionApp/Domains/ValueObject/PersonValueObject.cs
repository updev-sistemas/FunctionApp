
namespace FunctionApp.Domains.ValueObject;

public record PersonValueObject(
    string? Id, 
    string? Name, 
    string? Email, 
    DateOnly? Birthday, 
    DateTime? CreatedAt, 
    DateTime? UpdatedAt);
