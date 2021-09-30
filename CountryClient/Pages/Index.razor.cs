using Microsoft.AspNetCore.Components;
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

    public class IndexBase:ComponentBase
    {
        [Inject]
        public HttpClient client { get; set; }

        public string UrlGetCountries { get; set; } = "http://localhost:7071/api/GetCountry";
        public List<CountriesDto> Countries { get; set; } = null;

        public EditModel Model { get; set; } = new EditModel();

        protected override async Task<Task> OnInitializedAsync()
        {

           

            return base.OnInitializedAsync();
        }

        public async Task GetCountriesByNameAsync()
        {
            var result = await client.GetFromJsonAsync<List<CountriesDto>>(UrlGetCountries+"?name="+ Model.CountryName);

         
            Countries = result;
            StateHasChanged();
        }
    }   
}
