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
    public static class GetFromTableStorage
    {

        [FunctionName("GetFromTableStorage")]

        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("get");



            var con = "DefaultEndpointsProtocol=https;AccountName=abbestorageazure;AccountKey=S+umdRcAu5ayCb+K/9KlaRrxxKs9klSwMM8NmnwT8DKNDxtdQEWWFzKjOtUJ7ND3mK+A6EkU1ob+FpnP3/Qz6g==;EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(con);



            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("savedCountries");


            TableQuery<SaveEntity> query = new TableQuery<SaveEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "save"));


            List<SaveEntity> resultList = new List<SaveEntity>();

            foreach (SaveEntity item in table.ExecuteQuerySegmentedAsync(query, null).Result)
            {

                SaveEntity newItem = new SaveEntity();
                newItem.CountryName = item.CountryName;

                resultList.Add(newItem);
            }

           


            return new OkObjectResult(resultList.Take(10));
        }
    }
    public static class SaveCountry
    {

        [FunctionName("SaveCountry")]

            public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
            {

            log.LogInformation("Saving");

            string name = req.Query["name"];




            // Define the row,
            string sRow = name + DateTime.Now;

            SaveEntity saveEntity = new SaveEntity("save", sRow);

            saveEntity.CountryName = name;

            var con = "DefaultEndpointsProtocol=https;AccountName=abbestorageazure;AccountKey=S+umdRcAu5ayCb+K/9KlaRrxxKs9klSwMM8NmnwT8DKNDxtdQEWWFzKjOtUJ7ND3mK+A6EkU1ob+FpnP3/Qz6g==;EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(con);

           

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("savedCountries");

            table.CreateIfNotExistsAsync();

            TableOperation insertOperation = TableOperation.Insert(saveEntity);

            table.ExecuteAsync(insertOperation);


            return new OkObjectResult("");
        }
    }
    public static class Country
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



      


    }
}
