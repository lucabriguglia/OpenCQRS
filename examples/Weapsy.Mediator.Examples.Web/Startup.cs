using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF;
using Weapsy.Mediator.EventStore.EF.SqlServer;
using Weapsy.Mediator.Examples.Domain.Commands;
using Weapsy.Mediator.Examples.Reporting.Queries;
using Weapsy.Mediator.Extensions;

namespace Weapsy.Mediator.Examples.Web
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMediator mediator, MediatorDbContext mediatorDbContext)
        {
            // Ensure Weapsy.Mediator database is installed.
            mediatorDbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context =>
            {
                // Create a sample product loading data from domain events.
                var product = await GettingStarted.CreateProduct(mediator);

                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
