using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaEchoApp
{
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
                var body = $"Runtime: {Environment.Version} - Received `{JsonConvert.SerializeObject(input)}` at {DateTime.UtcNow.TimeOfDay}. Context: {JsonConvert.SerializeObject(context)}";
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = body
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
    }
}
