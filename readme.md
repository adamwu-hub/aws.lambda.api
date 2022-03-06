# AWS Lambda function for API G/W integration test

## Lession learnt

* ver 1.0 (`APIGatewayProxyRequest`) and ver 2.0 (`APIGatewayHttpApiV2ProxyRequest`) Lambda request/response contract are different
* ~~AWS toolkit doesn't support create and deploy .net 6 lambda yet~~ latest AWS Toolkit + VS2022 support .net 6
* the `Amazon.Lambda.Serialization.SystemTextJson` uses Microsoft `System.Text.Json` serializer which won't deserialize object as string, such as the Dictionary<string, object>, it will convert the object into JsonElement, so when you read its value, you need to use `JsonElement.ToString()`.
* ver 1.0 lambda request can have more info in `requestContext.authorizer`, sucha as principal id and context key/value pairs, but ver 2.0 request won't have principal id, it will only contain context key/value pairs

## About the projects

The solution has 1 lambda authorizer and 2 lambda functions, one uses ver 1.0 API contract, one uses ver 2.0 API contract.

First deploy the 3 lambda functions into AWS (you can do it inside VS2022) and test them using built-in test tool, make sure there is no runtime error.

### LambdaAuthoriser

This lambda function mock an authentication service, the passing in AuthorizationToken value can be `Allow`, `Deny` - 403 error, any other value causes 500 error.

### LambdaEchoApp

This is a REST API lambda function, which uses the version 1.0 API contract. The function takes ver 1.0 request and put the entire request JSON into response so we can see exactly what data the function received from API G/W. Create a API Gateway for REST API and link to this lambda function and test it.

### LambdaEchoHttpApp

This app does same thing as `LambdaEchoApp`, but uses version 2.0 API contract. Create a API Gateway for HTTP API and link to this lambda function and test it.

### Authorization

For above the 2 newly created API Gateways, set the API Gateway authentication to use the `LambdaAuthoriser`.

Make a call to the API Gateway and check the response to have correct information in `requestContext.authorizer` field. That proves the API gateway calls authorizer and return auth context to it, and then it passes those info to the request of the Echo function.