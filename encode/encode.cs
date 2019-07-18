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

            string toEncrypt = req.Query["toencrypt"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            toEncrypt = toEncrypt ?? data?.toencrypt;

            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(toEncrypt);
            byte[] hash = sha1.ComputeHash(inputBytes);

            string encrypted = Convert.ToBase64String(hash);

            return encrypted != null
                ? (ActionResult)new OkObjectResult(encrypted)
                : new BadRequestObjectResult("Please pass a text to encrypt with SHA1 on the query string or in the request body");
        }
    }
}
