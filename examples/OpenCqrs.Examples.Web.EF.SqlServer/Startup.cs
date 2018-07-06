using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Examples.Domain.Commands;
using OpenCqrs.Examples.Reporting.Queries;
using OpenCqrs.Examples.Shared;
using OpenCqrs.Extensions;
using OpenCqrs.Store.EF;
using OpenCqrs.Store.EF.Extensions;
using OpenCqrs.Store.EF.SqlServer;

namespace OpenCqrs.Examples.Web.EF.SqlServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpContextAccessor();

            services
                .AddOpenCqrs(typeof(CreateProduct), typeof(GetProduct))
                .AddSqlServerProvider(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDispatcher dispatcher, DomainDbContext domainDbContext)
        {
            // Ensure OpenCqrs database is installed.
            domainDbContext.Database.Migrate();
            //app.UseOpenCqrs().EnsureDomainDbCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                // Create a sample product loading data from domain events.
                var product = await GettingStarted.CreateProduct(dispatcher);

                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
