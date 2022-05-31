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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using SysOT.Models;
using SysOT.Services;
using Newtonsoft.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;

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
            services.AddScoped<IEncService,EncService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SysOT - backend", Version = "v1" });
            });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            services.AddCors(setupAction =>
                setupAction.AddPolicy("Frontend",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowAnyOrigin()
                    .WithOrigins("https://localhost:3000")
                )
            );
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

            app.UseCors("Frontend");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            var linux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if(Configuration.GetValue<string>("migrate") == "true")
                Seed(app,env.ContentRootPath + (!linux ? "\\" : "/") + Configuration["Database:SeedFile"],Configuration["Database:SeedPassword"],Configuration["Database:Salt"]);
        }
        public void Seed(IApplicationBuilder app,string seed,string password,string salt){
            IMongoService mongoSeedService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IMongoService>();
            IEncService encService = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IEncService>();
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
                
                var saltBytes = Encoding.UTF8.GetBytes(salt.ToString());
                model.Users = model.Users.Select(x => { 
                    x.PasswordHash = encService.EncryptPassword(password);
                    return x;
                });
                mongoSeedService.InsertDocuments("Users",model.Users);
                Console.WriteLine("Users initialized ...");
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
