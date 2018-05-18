using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.EventStore.EF;
using Weapsy.Cqrs.EventStore.EF.SqlServer;
using Weapsy.Cqrs.Examples.Domain.Commands;
using Weapsy.Cqrs.Examples.Reporting.Queries;
using Weapsy.Cqrs.Examples.Shared;
using Weapsy.Cqrs.Extensions;

namespace Weapsy.Cqrs.Examples.Web.EF.SqlServer
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

            services.AddWeapsyCqrs(typeof(CreateProduct), typeof(GetProduct));
            services.AddWeapsyCqrsSqlServerEventStore(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDispatcher dispatcher, EventStoreDbContext eventStoreDbContext)
        {
            // Ensure Weapsy.Cqrs database is installed.
            eventStoreDbContext.Database.Migrate();

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
