# Azure-functions-example-using-docker

These are examples of a function in a docker file used in the this post 
( https://medium.com/@santiagograngetto/azure-functions-with-docker-3b9fc3bcc7f6 )
to show how to create an Azure function, pack it in a docker container and deploy
it in Azure

## Encode
This function reads a string from a query or body and returns an encrypted version using SHA1.
The string can be anything not null.

## Is Prime
This functions read a number from query and return a text saying if the given number is prime or not.
The number must be a positive integer and returns a bad request object telling to introduce valid data.
