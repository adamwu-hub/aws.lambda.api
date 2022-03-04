# AWS Lambda function for API G/W integration test

## Lession learnt

* ver 1.0 and ver 2.0 Lambda request/response contract are different
* ~~AWS toolkit doesn't support create and deploy .net 6 lambda yet~~ latest AWS Toolkit support .net 6
* the `Amazon.Lambda.Serialization.SystemTextJson` uses Microsoft `System.Text.Json` serializer which won't deserialize object as string, such as the Dictionary<string, object>, it will convert the object to JsonElement, so when you read its value, you need to use JsonElement.ToString().
* ver 1.0 lambda request can have more principal id and context key/value pairs from authorizer, but ver 2.0 request won't have principal id, it will only contain context key/value pairs

## About the projects

First deploy the 3 lambda functions into AWS (you can do it inside VS2022) and test them using built-in test tool, make sure there is no runtime error.

### LambdaAuthoriser

This lambda function mock an authentication service, the passing in AuthorizationToken value can be `Allow`, `Deny` - 403 error, any other value causes 500 error.

### LambdaEchoApp

This is a REST API lambda function, which uses the version 1.0 API contract. The function takes ver 1.0 request and put the entire request JSON into response so we can see exactly what data the function received from API G/W. Create a API Gateway for REST API and link to this lambda function.

Also set the API Gateway authentication to use the `LambdaAuthoriser`.

Make a call to the API Gateway and make sure the echo app response has the authentication result, from `requestContext.authorizer` field.

### LambdaEchoHttpApp

This app does same thing as `LambdaEchoApp`, but uses version 2.0 API contract. Create a API Gateway for HTTP API and link to this lambda function.

Also set the API Gateway authentication to use the `LambdaAuthoriser`

Make a call to the API Gateway and make sure the echo app request has the authentication result, from `requestContext.authorizer` field.
