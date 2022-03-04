using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaEchoHttpApp;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
    {
        try
        {
            var body = new RequestInfo
            {
                Runtime = Environment.Version,
                Request = input,
                Context = context,
                TimeSpan = DateTime.UtcNow.TimeOfDay
            };

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(body, serializeOptions);

            Console.WriteLine(json);

            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 200,
                Body = json
            };
        }
        catch (Exception ex)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 500,
                Body = ex.ToString()
            };
        }
    }

    class RequestInfo
    {
        public Version Runtime { get; set; }
        public APIGatewayHttpApiV2ProxyRequest Request { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public ILambdaContext Context { get; set; }
    }
}
