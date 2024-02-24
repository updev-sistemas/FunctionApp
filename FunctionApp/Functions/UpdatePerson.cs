using System.Net;
using AutoMapper;
using FunctionApp.Domains.Bson;
using FunctionApp.Domains.ValueObject;
using System.Text.Json;
using FunctionApp.Infrastructure.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Functions;

public class UpdatePerson(ILoggerFactory loggerFactory, IPersonRepository repository, IMapper mapper)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<UpdatePerson>();

    [Function("UpdatePerson")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var requestBody = await req.ReadAsStringAsync();
        ArgumentNullException.ThrowIfNull(requestBody);

        var data = JsonSerializer.Deserialize<PersonValueObject>(requestBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var person = mapper.Map<PersonBson>(data);
        var result = await repository.UpdateAsync(id, person);

        if (result is null)
        {
            var response = req.CreateResponse(HttpStatusCode.UnprocessableEntity);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person not updated.");

            return response;
        }
        else
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person updated successfully!");

            return response;
        }
    }
}
