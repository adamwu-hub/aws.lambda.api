using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaEchoApp;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
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

            var json = JsonConvert.SerializeObject(body, Formatting.Indented);
            Console.WriteLine(json);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = json
            };
        }
        catch (Exception ex)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = ex.ToString()
            };
        }
    }

    class RequestInfo
    {
        public Version Runtime { get; set; }
        public APIGatewayProxyRequest Request { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public ILambdaContext Context { get; set; }
    }
}
