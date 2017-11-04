using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.Examples.Queries;

namespace Weapsy.Mediator.Examples.AspNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Use Scrutor to register all services
            services.Scan(s => s
                .FromAssembliesOf(typeof(IMediator), typeof(Something))
                .AddClasses()
                .AsImplementedInterfaces());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMediator mediator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var something = await mediator.GetResultAsync<GetSomething, Something>(new GetSomething());
                await context.Response.WriteAsync(something.Name);
            });
        }
    }
}
