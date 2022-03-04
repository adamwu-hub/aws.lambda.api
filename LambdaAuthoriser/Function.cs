using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaAuthoriser;

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
            var body = $"Runtime: {Environment.Version} - Received `{JsonConvert.SerializeObject(input, Formatting.Indented)}` at {DateTime.UtcNow.TimeOfDay}. Context: {JsonConvert.SerializeObject(context, Formatting.Indented)}";

            Console.WriteLine(body);

            if (string.IsNullOrWhiteSpace(input?.AuthorizationToken))
            {
                throw new InvalidDataException($"{nameof(input.AuthorizationToken)} cannot be null");
            }

            if (string.IsNullOrWhiteSpace(input?.MethodArn))
            {
                throw new InvalidDataException($"{nameof(input.MethodArn)} cannot be null");
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
                            Effect = input.AuthorizationToken,
                            Resource = new HashSet<string>{ input.MethodArn }
                        }
                    }
                }
            };
        }
        catch
        {
            throw;
        }
    }
}
