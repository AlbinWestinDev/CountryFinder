using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;
using Shared;

namespace Function1
{

   
    public static class GetCountry
    {
        [FunctionName("GetCountry")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string name = req.Query["name"];


            HttpClient client = new HttpClient();
            List<CountriesDto> countries = new List<CountriesDto>();
            var response = await client.GetAsync("https://devscenrum.azurewebsites.net/api/v1/geografi/land");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CountriesDto>>(jsonString);

                if (!string.IsNullOrEmpty(name))
                {
                    countries = result.Where(x => x.Namn.ToLower().StartsWith(name.ToLower())).ToList();
                }
                
            }


            return new OkObjectResult(countries);
        }



        [FunctionName("SaveCountry")]
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ILogger log)
        {

            log.LogInformation("Saving");


            dynamic body = await req.Content.ReadAsStringAsync();
            var c = JsonConvert.DeserializeObject<SavedCountriesDto>(body as string);


            // Define the row,
            string sRow = c.Name + c.Created;

            SaveEntity saveEntity = new SaveEntity("save",sRow);

            saveEntity.CountryName = c.Name;

            


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("https://azurefunctionswebapp.azurewebsites.net");

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("savedCountries");

            table.CreateIfNotExistsAsync();

            TableOperation insertOperation = TableOperation.Insert(saveEntity);

            table.ExecuteAsync(insertOperation);

          
            return req.CreateResponse(HttpStatusCode.OK, "Ok");
        }


    }
}
