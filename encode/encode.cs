using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace encode
{
    public static class encode
    {
        [FunctionName("encode")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // read the parameter in query
            string toEncrypt = req.Query["toencrypt"];

            // read the parameter in body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            // use the body data if the data from query is empty
            toEncrypt = toEncrypt ?? data?.toencrypt;

            // create encrypter object
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            // encrypt
            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(toEncrypt);
            byte[] hash = sha1.ComputeHash(inputBytes);

            // convert to string
            string encrypted = Convert.ToBase64String(hash);

            // return encrypted data or error if the data is null
            return encrypted != null
                ? (ActionResult)new OkObjectResult(encrypted)
                : new BadRequestObjectResult("Please pass a text to encrypt with SHA1 on the query string or in the request body");
        }
    }
}
