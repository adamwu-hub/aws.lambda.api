using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

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
        public LambdaJsonResponse FunctionHandler(object input, ILambdaContext context)
        {
            try
            {
                var body = $"Runtime: {Environment.Version} - Received `{input}` at {DateTime.UtcNow.TimeOfDay}. Context: {JsonConvert.SerializeObject(context)}";
                return new LambdaJsonResponse
                {
                    statusCode = 200,
                    body = body
                };
            }
            catch (Exception ex)
            {
                return new LambdaJsonResponse
                {
                    statusCode = 500,
                    body = ex.ToString()
                };
            }
        }

        public class LambdaJson
        {
            public Dictionary<string, string> headers { get; set; }
            public string body { get; set; }
        }

        public class LambdaJsonRequest : LambdaJson
        {
            public Dictionary<string, string> queryStringParameters { get; set; }
            public string requestContext { get; set; }
        }

        public class LambdaJsonResponse : LambdaJson
        {
            public int statusCode { get; set; }
            public bool isBase64Encoded { get; set; }
        }
    }
}