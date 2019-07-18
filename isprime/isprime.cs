using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace isprime
{
    public static class isprime
    {
        [FunctionName("isprime")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Retrieve the number from the querry as text
            string numberAsText = req.Query["number"];

            // Validate the parameter
            if (string.IsNullOrWhiteSpace(numberAsText))
            {
                return new BadRequestObjectResult("Please pass a valid number on the query");
            }

            int number;

            try
            {
                number = Int32.Parse(numberAsText);
            } catch (FormatException e)
            {
                return new BadRequestObjectResult("The number must be an integer");
            }

            if (number < 1)
            {
                return new BadRequestObjectResult("The number must be greater than 0");
            }

            // Check if the number is prime
            Boolean isPrime = true; //flag

            for (int i = 2; i < number; i++)
            {
                // If the given number is divisible by any number greater than 1 
                // or lesser than itself its not prime
                if (number % i == 0) 
                {
                    isPrime = false;
                }
            }

            // Build response
            if (isPrime)
            {
                return new OkObjectResult("The number " + number + " is prime");
            } else
            {
                return new BadRequestObjectResult("The number " + number + " is NOT prime");
            }
        }
    }
}
