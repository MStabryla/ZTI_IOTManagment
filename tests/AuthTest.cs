using System;
using System.Net.Http.Json;
using Xunit;
using SysOT.Models;
using System.Threading.Tasks;
using SysOT.Tests.Models;

namespace SysOT.Tests
{
    public class AuthTest : IClassFixture<TestingWebAppFactory>
    {
        private TestingWebAppFactory factory;
        public AuthTest(TestingWebAppFactory _factory){
            factory = _factory;
        }

        [Fact]
        public async Task Login()
        {
            var client = factory.CreateClient();
            LoginModel model = new LoginModel(){ Email = "main_admin@sysot.com", Password = "zaq1@WSX"};
            var postContent = JsonContent.Create(model);
            var response = await client.PostAsync("auth/login",postContent);
            var responseData = await response.Content.ReadFromJsonAsync<TokenModel>();
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
            Assert.NotNull(responseData.Token);
        }
    }
}
