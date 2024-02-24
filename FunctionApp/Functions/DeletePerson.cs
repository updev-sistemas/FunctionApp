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

public class DeletePerson(ILoggerFactory loggerFactory, IPersonRepository repository)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DeletePerson>();

    [Function("DeletePerson")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "person/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var requestBody = await req.ReadAsStringAsync();
        ArgumentNullException.ThrowIfNull(requestBody);

        var result = await repository.RemoveAsync(id);

        if (result)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person deleted successfully!");

            return response;
        }
        else
        {
            var response = req.CreateResponse(HttpStatusCode.UnprocessableEntity);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Person not updated.");

            return response;
        }
    }
}
