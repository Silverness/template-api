using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using xnamespace.configuration;

namespace xnamespace
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Service Configuration
            var serviceConfiguration = new ServiceConfiguration();

            _configuration.Bind("ServiceConfiguration", serviceConfiguration);

            services.TryAddSingleton<IServiceConfiguration>(serviceConfiguration);

            services.AddMvcCore().AddVersionedApiExplorer(x => x.GroupNameFormat = "'v'V");

            services.AddApiVersioning(x =>
            {
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        new Info
                        {
                            Title = $"{serviceConfiguration.ServiceName} v{description.ApiVersion}",
                            Version = description.ApiVersion.ToString()
                        });
                }
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.CustomSchemaIds(c => c.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger(x =>
            {
                x.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);

            });

            app.UseSwaggerUI(options =>
            {
                foreach (var apiVersionDescription in (provider.ApiVersionDescriptions))
                {
                    options.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.josn", apiVersionDescription.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
