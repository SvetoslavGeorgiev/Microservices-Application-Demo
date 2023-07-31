namespace Commands.Web.Controllers
{
    using Commands.Web.Models.Platforms;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http.Headers;

    public class PlatformsController : Controller
    {
        private readonly IConfiguration configuration;

        public PlatformsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {

            string apiHttpsUrl = configuration["PlatformServicesAllPlatformsHttps"]!;
            string apiHttpUrl = configuration["PlatformServicesAllPlatformsHttp"]!;
            string k8sUrl = configuration["PlatformServicesAllPlatformsK8S"]!;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(k8sUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(k8sUrl);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();
                var platforms = JsonConvert.DeserializeObject<IEnumerable<PlatformsViewModel>>(data)!;

                return View(platforms);
            }
        }
    }

}
