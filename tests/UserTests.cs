using System;
using System.Net.Http.Json;
using Xunit;
using SysOT.Models;
using System.Threading.Tasks;
using System.Linq;
using SysOT.Tests.Models;

namespace SysOT.Tests
{
    public class UsersTests : IClassFixture<TestingWebAppFactory>
    {
        private TestingWebAppFactory factory;
        public UsersTests (TestingWebAppFactory _factory){
            factory = _factory;
        }

        [Fact]
        public async Task GetUsers()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");
            var response = await client.GetAsync("user");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<User[]>();
            Assert.NotEmpty(responseData);
        }

        [Fact]
        public async Task GetUsersFalse()
        {
            var client = await factory.GetAuthorizedClient("manager@sysot.com");
            var response = await client.GetAsync("user");
            Assert.Equal(System.Net.HttpStatusCode.Forbidden,response.StatusCode);

        }

        [Fact]
        public async Task GetUserModify()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");
            var response = await client.GetAsync("user");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<User[]>();
            Assert.NotEmpty(responseData);

            var user = responseData[0];
            var newUserRoles =  user.Roles.ToList(); newUserRoles.Add("Tester");
            user.Roles = newUserRoles.ToArray();

            var content = JsonContent.Create(user);
            response = await client.PutAsync("/user/" + user.Id,content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }
    }
}
