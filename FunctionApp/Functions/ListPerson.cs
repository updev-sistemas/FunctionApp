using System.Net;
using System.Text.Json;
using AutoMapper;
using FunctionApp.Domains.ValueObject;
using FunctionApp.Infrastructure.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FunctionApp.Functions;

public class ListPerson(ILoggerFactory loggerFactory, IPersonRepository repository, IMapper mapper)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ListPerson>();

    [Function("ListAllPeople")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "list-people")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Retrieve query parameters

        var people = await repository.GetAsync(CancellationToken.None);

        var responseContentJson = new
        {
            Data = mapper.Map<IEnumerable<PersonValueObject>>(people),
            Total = await repository.CountAsync(CancellationToken.None)
        };

        // Create response
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        response.WriteString(JsonSerializer.Serialize(responseContentJson));

        return response;
    }
}
