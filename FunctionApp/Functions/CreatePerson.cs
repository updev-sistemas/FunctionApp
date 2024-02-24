using AutoMapper;
using FunctionApp.Domains.Bson;
using FunctionApp.Domains.ValueObject;
using FunctionApp.Infrastructure.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunctionApp.Functions;

public class CreatePerson(ILoggerFactory loggerFactory, IPersonRepository repository, IMapper mapper)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<CreatePerson>();

    [Function("RegisterPerson")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "person")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var requestBody = await req.ReadAsStringAsync();
        ArgumentNullException.ThrowIfNull(requestBody);

        var data = JsonSerializer.Deserialize<PersonValueObject>(requestBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var person = mapper.Map<PersonBson>(data);
        await repository.CreateAsync(person);

        if (string.IsNullOrEmpty(person.Id))
        {
            var response = req.CreateResponse(HttpStatusCode.UnprocessableEntity);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person not created.");

            return response;
        }
        else
        {
            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person created successfully!");

            return response;
        }
    }
}
