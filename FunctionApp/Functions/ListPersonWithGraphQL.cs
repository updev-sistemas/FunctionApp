using System.Net;
using Amazon.Runtime.Internal.Util;
using FunctionApp.Domains.Bson;
using FunctionApp.Infrastructure.Query;
using FunctionApp.Infrastructure.Repository;
using HotChocolate.Data.MongoDb;
using HotChocolate.Execution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FunctionApp.Functions;

public class ListPersonWithGraphQL(ILoggerFactory loggerFactory)
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger = loggerFactory.CreateLogger<ListPersonWithGraphQL>();
    private static readonly ExecutionStrategy DefaultExecutionStrategy = new MaxParallelExecutionStrategy();

    [Function("graphql")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");


        var schema = SchemaBuilder.New()
             .AddQueryType<PersonQuery>()
             .AddMongoDbFiltering()
             .AddMongoDbSorting()
             .Create();

        var result = await schema.ExecuteRequestAsync(req.Body, _logger,
            services => services.AddSingleton<IMongoCollection<PersonBson>>(
                new MongoClient("mongodb://person-sa:Ok96cD3mSglg9C6LAugffChNOfKky5X16YzXCuaclTyMrMlEnT3GTtPyr8tq5MGXEswpTgTq8cdDACDb5nbBpQ==@person-sa.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@person-sa@")
                    .GetDatabase("person-sa")
                    .GetCollection<PersonBson>("people")),
            context,
            DefaultExecutionStrategy);

        var response = req.CreateResponse();

        if (result is IReadOnlyQueryResult queryResult)
        {
            response.WriteBody(queryResult.ToJson());
        }
        else
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.WriteString(result.ToString());
        }

        return response;
    }
}
