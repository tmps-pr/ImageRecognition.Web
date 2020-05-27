using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceId.Web.Api.Options;
using FaceId.Web.Api.Services;
using FaceId.Web.Core;
using FaceId.Web.Core.Abstract;
using FaceId.Web.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;

namespace FaceId.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(AssemblyMarker));
            services.AddPredictionEnginePool<GenderInMemoryImageData, Prediction>()
                    .FromFile(Configuration["MLModel:GenderModelFilePath"]);
            services.AddPredictionEnginePool<AgeInMemoryImageData, Prediction>()
                   .FromFile(Configuration["MLModel:AgeModelFilePath"]);

            services.AddOptions<BlobStorageOption>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection("BlobStorage").Bind(settings);
               });

            services.AddScoped<IFileServiceInitializer, BlobStorageFileService>();
            services.AddScoped<IFileService, BlobStorageFileService>();
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["AllowedHosts"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();

            var provider = services.BuildServiceProvider();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Image recognition API", Version = "v1" });
            });
            var fileServiceInitializer = provider.GetRequiredService<IFileServiceInitializer>();
            await fileServiceInitializer.InitializeAsync(Constants.GenderContainer);
            await fileServiceInitializer.InitializeAsync(Constants.AgeContainer);
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Image Recognition V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
