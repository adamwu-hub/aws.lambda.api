# AWS Lambda function for API G/W integration test

## Lession learnt

* ver 1.0 and ver 2.0 Lambda request/response contract are different
* ~~AWS toolkit doesn't support create and deploy .net 6 lambda yet~~ latest AWS Toolkit support .net 6
* .net core 3.1 lambda need to use `Amazon.Lambda.Serialization.Json`, otherwise Dictionary values are not deserialized correctly
* .net 6 lambda can use `Amazon.Lambda.Serialization.SystemTextJson` which handles Dictionary values correctly
* ver 1.0 lambda request can have more principal id and context key/value pairs from authorizer, but ver 2.0 request won't have principal id, it will only contain context key/value pairs

## About the projects

First deploy the 3 lambda functions into AWS and test them using built-in test tool, make sure there is no runtime error.

### LambdaAuthoriser

This lambda function mock an authentication service, the passing in AuthorizationToken value can be `Allow`, `Deny`, any other value causes error.

### LambdaEchoApp

This is a REST API lambda function, which uses the version 1.0 API contract. Create a API Gateway for REST API and link to this lambda function.

Also set the API Gateway authentication to use the `LambdaAuthoriser`.

Make a call to the API Gateway and make sure the echo app request has the authentication result, such as the context array.

### LambdaEchoHttpApp

This is a HTTP API lambda function, which uses the version 2.0 API contract. Create a API Gateway for HTTP API and link to this lambda function.

Also set the API Gateway authentication to use the `LambdaAuthoriser`

Make a call to the API Gateway and make sure the echo app request has the authentication result, such as the context array.
