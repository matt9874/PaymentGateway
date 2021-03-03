# Payment Gateway

Please note that Swagger specification can be found [here](http://localhost:5000/swagger/PaymentGatewayOpenApiSpecification/swagger.json) when the application is running.

## Prerequisites
Please install .NET Core 3.1 from 
```bash
https://dotnet.microsoft.com/download/dotnet-core/3.1
```
To easily send requests, you may want to install Postman:
```bash
https://www.postman.com/downloads/
```
Alternatively: 
* curl can be used to send requests.
* the Swagger UI can be found [here](http://localhost:5000/swagger/index.html) when the application is running 

## Build and execute
Open a command prompt and navigate to the PaymentGateway.API folder. Build the application by executing
```bash
dotnet build PaymentGateway.API.csproj -c Release
```
change folder to the application
```bash
cd bin/release/netcoreapp3.1
```
And run the service  by executing
```bash
dotnet PaymentGateway.API.dll
```
The application will be listening on http://localhost:5000  
To stop the service, press Ctrl+C

## Run application using Docker
Navigate to PaymentGateway folder (which contains DockerFile) and execute the following:
```bash
docker build -t paymentgateway .
```
```bash
docker run -d -p 5000:80 --name paymentgatewayapp paymentgateway
```
Application should now be listening on port 5000

## Send POST request  
Send a post message to http://localhost:5000/api/merchants/{merchantId}/payments where {merchantId} is an integer variable
* The only valid merchantIds are 1, 2 and 3
* Content-Type should be set to "application/json"
* Accept header should be set to "*/*"" or "application/json" .
* Please see below for an example of how the body should look:
  * body should be a json object with the following properties
    * "amount" must be a decimal of at least 0.01
    * "cvv": must be valid cvv string
    * "expiryMonth": must be integer between 1 and 12
    * "expiryYear": must be an integer (representing the Year AD)
    * "cardNumber": must be a string and a valid credit card number,
    * "currency" can be "PoundsSterling" or "Euro"
* The client should easily be able to navigate to the PaymentDetails by using either of the following in the POST response
  * Location header
  * links property in the body
  
Here is an example of the body for a post message:
```json
{
    "amount": 0.01,
    "cvv": "444",
    "expiryMonth": 3,
    "expiryYear": 2022,
    "cardNumber": "4012888888881881",
    "currency": "PoundsSterling"
}
```

## Send GET request
Send a GET message to http://localhost:5000/api/merchants/{merchantId}/payments/{paymentId} where {merchantId} and {paymentId} are integer and long variables respectively

## Bank Simulator
The simulation of the bank is performed by the DummyBankClient which randomly returns a success (8/9) of the time and a failure (1/9) of the time.

## Assumptions
* Assumed that the merchant signup process is taken care of in another part of the system. 
* Assumed that a bank response returns a unique identifier of type long whether or not the payment was successful.
* Assumed that a 422 should be returned when input validation fails.
* Assumed that a 201 should be returned when the payment is unsuccessful. The body of the response can be inspected to see whether the payment was successful (or the Location header or links can be used to get the URI of the payment details).
* When returning payment details, assumed that CVV and expiry month should not be returned.
