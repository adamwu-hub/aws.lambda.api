using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaAuthoriser
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayCustomAuthorizerResponse FunctionHandler(APIGatewayCustomAuthorizerRequest input, ILambdaContext context)
        {
            try
            {
                var body = $"Runtime: {Environment.Version} - Received `{JsonConvert.SerializeObject(input)}` at {DateTime.UtcNow.TimeOfDay}. Context: {JsonConvert.SerializeObject(context)}";

                Console.WriteLine(body);

                var basicAuth = input?.AuthorizationToken;
                if (string.IsNullOrWhiteSpace(basicAuth))
                {
                    throw new Exception();
                }

                var authCtx = new APIGatewayCustomAuthorizerContextOutput();
                authCtx["a"] = "context-a";
                authCtx["b"] = "context-b";
                authCtx["c"] = "context-c";

                return new APIGatewayCustomAuthorizerResponse
                {
                    PrincipalID = "wuad",
                    UsageIdentifierKey = Guid.NewGuid().ToString(),
                    Context = authCtx,
                    // related doc: https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_policies_elements.html
                    PolicyDocument = new APIGatewayCustomAuthorizerPolicy
                    {
                        //Version = "0.0.1",
                        Statement = new List<APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement>
                        {
                            new APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement
                            {
                                Action = new HashSet<string>{ "execute-api:Invoke" },
                                Effect = basicAuth,
                                Resource = new HashSet<string>{ input.MethodArn }
                            }
                        }
                    }
                };
            }
            catch
            {
                throw;
                //return new APIGatewayCustomAuthorizerResponse
                //{
                //    StatusCode = 500,
                //    Body = ex.ToString()
                //};
            }
        }
    }
}
