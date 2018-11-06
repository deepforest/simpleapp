using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors;

namespace SimpleApp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseMvc();

            var swaggerRoute = "/swagger/v1.0.json";
            app.UseSwaggerWithApiExplorer(config =>
            {
                config.GeneratorSettings.OperationProcessors.TryGet<ApiVersionProcessor>().IncludedVersions = new[] { "1.0" };
                config.GeneratorSettings.Title = "Web APIs";
                config.GeneratorSettings.Description = "Exposes RESTfull apis for web client";
                config.SwaggerRoute = swaggerRoute;
            });

            app.UseSwaggerUi3(config =>
            {
                config.SwaggerRoutes.Add(new SwaggerUi3Route("v1.0", swaggerRoute));
                config.SwaggerUiRoute = "/swagger";
                config.DocExpansion = "list";
                config.ValidateSpecification = true;
            });
        }
    }
}
