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

            var responseData = await response.Content.ReadFromJsonAsync<UserModel[]>();
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
        public async Task UserAddAndRemove()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");

            var user = new UserModel(){
                Name = "TestUser_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString(),
                Email = ("TestUser_" + DateTime.Now.ToShortDateString()).ToLower() + "_@sysot.com",
                PasswordHash = "zaq1@WSX",
                Roles = new string[] {"User"}
            };
            var content = JsonContent.Create(user);
            var response = await client.PostAsync("/user/",content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<UserModel>();
            Assert.NotNull(responseData.Id);
            Assert.NotEqual(user.PasswordHash,responseData.PasswordHash);

            response = await client.DeleteAsync("/user/" + responseData.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }

        [Fact]
        public async Task UserModify()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");
            var response = await client.GetAsync("user");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<UserModel[]>();
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
