using SysOT;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using SysOT.Models;
using System.Net.Http.Json;
using SysOT.Tests.Models;

namespace SysOT.Tests
{
    public class TestingWebAppFactory : WebApplicationFactory<Startup>
    {
        private const string _seedPassword = "zaq1@WSX";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }
        public async Task<HttpClient> GetAuthorizedClient(string email,string password = _seedPassword){
            var client = CreateClient();
            LoginModel model = new LoginModel(){ Email = email, Password = password};
            var postContent = JsonContent.Create(model);
            var response = await client.PostAsync("auth/login",postContent);
            var responseData = await response.Content.ReadFromJsonAsync<TokenModel>();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",responseData.Token);
            return client;
        }
    }
}