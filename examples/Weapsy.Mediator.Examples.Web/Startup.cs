using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF;
using Weapsy.Mediator.EventStore.EF.Extensions;
using Weapsy.Mediator.Examples.Domain.Commands;
using Weapsy.Mediator.Examples.Reporting.Queries;
using Weapsy.Mediator.Extensions;

namespace Weapsy.Mediator.Examples.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Register Weapsy.Mediator and dependent assemblies.
            services.AddWeapsyMediator(typeof(CreateProduct), typeof(GetProduct));

            // Add entity framework event store.
            services.AddWeapsyEFEventStore();

            // Any of the data providers supported by entity framework core can be used.
            services.AddDbContext<MediatorDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EventStore;Trusted_Connection=True;MultipleActiveResultSets=true"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMediator mediator, MediatorDbContext mediatorDbContext)
        {
            // Ensure Weapsy.Mediator database is installed.
            mediatorDbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
