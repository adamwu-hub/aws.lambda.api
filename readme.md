# AWS Lambda function for API G/W integration test

## Lession learnt

* ver 1.0 and ver 2.0 Lambda request/response contract are different
* AWS toolkit doesn't support create and deploy .net 6 lambda yet
* .net core 3.1 lambda need to use `Amazon.Lambda.Serialization.Json`, otherwise Dictionary values are not deserialized correctly
* .net 6 lambda can use `Amazon.Lambda.Serialization.SystemTextJson` which handles Dictionary values correctly
* ver 1.0 lambda request can have more principal id and context key/value pairs from authorizer, but ver 2.0 request won't have principal id, it will only contain context key/value pairs