using Kledex.Bus.RabbitMQ.Extensions;
using Kledex.Examples.Domain.Commands;
using Kledex.Examples.Reporting.Queries;
using Kledex.Examples.Shared;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Kledex.Store.EF.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Examples.Web.EF.SqlServer
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
                .AddKledex(typeof(CreateProduct), typeof(GetProduct))
                .AddOptions(opt =>
                {
                    opt.PublishEvents = true;
                    opt.SaveCommandData = false;
                })
                .AddSqlServerProvider(Configuration)
                .AddRabbitMQProvider(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDispatcher dispatcher)
        {
            // Ensure Kledex database is installed.
            app.UseKledex().EnsureDomainDbCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Create a sample product loading data from domain events.
            var product = GettingStarted.CreateProduct(dispatcher).GetAwaiter().GetResult();

            app.Run(async context =>
            {
                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
