using Microsoft.AspNetCore.Components;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CountryClient.Pages
{

    public class EditModel
    {
        public string CountryName { get; set; }
    }

    public class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient client { get; set; }

        public string UrlGetCountries { get; set; } = "http://localhost:7071/api/GetCountry";
        public List<CountriesDto> Countries { get; set; } = null;

        public EditModel Model { get; set; } = new EditModel();

        public string UrlLastSearches { get; set; } = "http://localhost:7071/api/GetFromTableStorage";

        public List<SaveEntity> LastSearches { get; set; } = new List<SaveEntity>();
        public string UrlSaveLastSearches { get; set; } = "http://localhost:7071/api/SaveCountry";

        protected override async Task<Task> OnInitializedAsync()
        {


           await GetLastSearches();
            return base.OnInitializedAsync();
        }

        public async Task GetCountriesByNameAsync()
        {
            var result = await client.GetFromJsonAsync<List<CountriesDto>>(UrlGetCountries+"?name="+ Model.CountryName);

            await client.GetAsync(UrlSaveLastSearches + "?name=" + Model.CountryName);
            await GetLastSearches();
         
            Countries = result;
            StateHasChanged();
        }
        public async Task GetLastSearches()
        {
            LastSearches = await client.GetFromJsonAsync<List<SaveEntity>>(UrlLastSearches);


            StateHasChanged();

        }
    }   
}
