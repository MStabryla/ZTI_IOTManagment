using System;
using System.Net.Http.Json;
using Xunit;
using SysOT.Models;
using System.Threading.Tasks;
using System.Linq;
using SysOT.Tests.Models;

namespace SysOT.Tests
{
    public class DeviceTests : IClassFixture<TestingWebAppFactory>
    {
        private TestingWebAppFactory factory;
        public DeviceTests (TestingWebAppFactory _factory){
            factory = _factory;
        }

        [Fact]
        public async Task GetDevices()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");
            var response = await client.GetAsync("devices");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<Device[]>();
            Assert.NotEmpty(responseData);
        }

        [Fact]
        public async Task DeviceAddAndRemove()
        {
            var client = await factory.GetAuthorizedClient("main_admin@sysot.com");

            var device = new Device(){
                DeviceName = "TestDevice1",
                IpAddress = "192.168.76.54",
                Type = "test",
                Description = "test description"
            };
            var content = JsonContent.Create(device);
            var response = await client.PostAsync("/devices/",content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData2 = await response.Content.ReadFromJsonAsync<Device>();
            Assert.NotNull(responseData2.Id);
            Assert.NotEmpty(responseData2.Managers);

            response = await client.DeleteAsync("/devices/" + responseData2.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }

        [Fact]
        public async Task TestPutDevice()
        {
            var client = await factory.GetAuthorizedClient("manager@sysot.com");
            var response = await client.GetAsync("devices");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<Device[]>();
            Assert.NotEmpty(responseData);

            var device = new Device(){
                DeviceName = "TestDevice1",
                IpAddress = "192.168.76.54",
                Type = "test",
                Description = "test description"
            };
            var content = JsonContent.Create(device);
            response = await client.PostAsync("/devices/",content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData2 = await response.Content.ReadFromJsonAsync<Device>();
            Assert.NotNull(responseData2.Id);

            device.Id = responseData2.Id;
            device.Mobile = true;
            device.Managers = new string[] {"b3BlbnNzaC1rZXktdjEAAAA3"};

            content = JsonContent.Create(device);
            response = await client.PutAsync("/devices/" + device.Id, content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            response = await client.DeleteAsync("/devices/" + responseData2.Id);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden,response.StatusCode);

            var admin = await factory.GetAuthorizedClient("main_admin@sysot.com");

            response = await admin.DeleteAsync("/devices/" + responseData2.Id);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
        }
    }
}
