using System.Net;
using System.Text.Json;
using AutoMapper;
using FunctionApp.Domains.ValueObject;
using FunctionApp.Infrastructure.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.IO;

namespace FunctionApp.Functions;

public class GetPerson(ILoggerFactory loggerFactory, IPersonRepository repository, IMapper mapper)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetPerson>();

    [Function("GetPersonById")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var requestBody = await req.ReadAsStringAsync();
        ArgumentNullException.ThrowIfNull(requestBody);

        var result = await repository.GetAsync(id, CancellationToken.None);

        if (result is null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person not found.");

            return response;
        }
        else
        {
            var person = mapper.Map<PersonValueObject>(result);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(JsonSerializer.Serialize(person));

            return response;
        }
    }
}
