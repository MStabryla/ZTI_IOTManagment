using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;
using Newtonsoft.Json;

namespace SysOT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddScoped<IMongoService,MongoService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SysOT - backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if(Configuration.GetValue<string>("migrate") == "true")
                Seed(app,env.ContentRootPath + "\\" + Configuration["Database:SeedFile"]);
        }
        public void Seed(IApplicationBuilder app,string seed){
            IMongoService mongoSeedService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IMongoService>();
            var query = new BsonDocument {};
            
            if(mongoSeedService.GetDocuments<Device>("Devices",query).Count() <= 0)
            {
                Console.WriteLine("Seed started ...");

                string seedStringModel = System.IO.File.ReadAllText(seed);
                SeedModel model = JsonConvert.DeserializeObject<SeedModel>(seedStringModel);

                mongoSeedService.InsertCollection("Devices");
                mongoSeedService.InsertCollection("MeasurementTypes");
                mongoSeedService.InsertCollection("MeasurementBuckets");
                Console.WriteLine("Collection initialized ...");
                
                mongoSeedService.InsertDocuments("Devices",model.Devices);
                Console.WriteLine("Devices initialized ...");
                mongoSeedService.InsertDocuments("MeasurementTypes",model.MeasurementTypes);
                Console.WriteLine("MeasurementTypes initialized ...");
                mongoSeedService.InsertDocuments("MeasurementBuckets",model.Measurements);
                Console.WriteLine("MeasurementBuckets initialized ...");

                Console.WriteLine("Seed finished.");
            }
        }
    }
}
