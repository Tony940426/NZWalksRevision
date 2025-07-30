using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.Dto;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
             List<RegionsDto> response = new List<RegionsDto>();  
            try
            {
                //Get all Regions from the API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7231/api/Regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDto>>());

            }
            catch (Exception)
            {

                throw;
            }

            return View(response);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel addRegionViewModel)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7231/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel),Encoding.UTF8, "application/json")
            };


           var httpResponseMessage = await client.SendAsync(httpRequestMessage);
           httpResponseMessage.EnsureSuccessStatusCode();
           
           var response =  await httpResponseMessage.Content.ReadFromJsonAsync<RegionsDto>();
            if (response != null) 
            { 
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionsDto>($"https://localhost:7231/api/Regions/{id.ToString()}");

            if (response is not null )
            {
                return View(response);
            }
            return View(null);

        }
    }
}
