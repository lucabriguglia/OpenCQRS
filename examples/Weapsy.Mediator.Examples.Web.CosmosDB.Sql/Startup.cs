using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Configuration;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Extensions;
using Weapsy.Mediator.Examples.Domain.Commands;
using Weapsy.Mediator.Examples.Reporting.Queries;
using Weapsy.Mediator.Examples.Shared;
using Weapsy.Mediator.Extensions;

namespace Weapsy.Mediator.Examples.Web.CosmosDB.Sql
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

            services.AddWeapsyMediator(typeof(CreateProduct), typeof(GetProduct));
            services.AddWeapsyMediatorEventStore(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMediator mediator, IOptions<CosmosDBSettings> settings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.EnsureEventStoreDbCreated(settings);

            app.Run(async (context) =>
            {
                // Create a sample product loading data from domain events.
                var product = await GettingStarted.CreateProduct(mediator);

                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
