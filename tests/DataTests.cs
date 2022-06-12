using System;
using System.Net.Http.Json;
using Xunit;
using SysOT.Models;
using System.Threading.Tasks;
using System.Linq;
using SysOT.Tests.Models;

namespace SysOT.Tests
{
    public class DataTests : IClassFixture<TestingWebAppFactory>
    {
        private TestingWebAppFactory factory;
        public DataTests (TestingWebAppFactory _factory){
            factory = _factory;
        }

        [Fact]
        public async Task InsertData(){
            var client = await factory.GetAuthorizedClient("device_03@devices.sysot.com");
            var data = new MeasurementModel(){
                Value = "15.3",
                Time = DateTime.UtcNow,
                Type = new MeasurementType(){
                    TypeName = "CO gas dencity",
                    VariableType = "float",
                    Numeric = true
                }
            };
            var content = JsonContent.Create(data);
            var response = await client.PostAsync("data",content);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
            var bucket = await response.Content.ReadFromJsonAsync<MeasurementBucket>();
            Assert.True(bucket.Values.Any(x => x.Time == data.Time));
        }
        [Fact]
        public async Task GetData(){
            var client = await factory.GetAuthorizedClient("manager@sysot.com");
            var response = await client.GetAsync("devices");
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<Device[]>();
            Assert.NotEmpty(responseData);

            response = await client.GetAsync("/data/" + responseData[1].Id);
            Assert.Equal(System.Net.HttpStatusCode.OK,response.StatusCode);
            var buckets = await response.Content.ReadFromJsonAsync<MeasurementBucket[]>();
            Assert.NotEmpty(buckets);
        }
    }
}