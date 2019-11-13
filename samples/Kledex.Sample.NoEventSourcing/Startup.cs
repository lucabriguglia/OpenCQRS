using Kledex.Bus.ServiceBus.Extensions;
using Kledex.Caching.Memory;
using Kledex.Extensions;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain;
using Kledex.Store.Cosmos.Sql.Configuration;
using Kledex.Store.EF.Extensions;
using Kledex.Store.EF.SqlServer;
using Kledex.UI.Extensions;
using Kledex.Validation.FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kledex.Sample.NoEventSourcing
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
            services.AddRazorPages();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<SampleDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SampleDb")));

            services
                .AddKledex(options =>
                {
                    options.PublishEvents = true;
                    options.SaveCommandData = true;
                    options.ValidateCommands = false;
                }, typeof(Product))
                .AddSqlServerProvider(Configuration)
                .AddServiceBusProvider()
                .AddFluentValidationProvider()
                .AddMemoryCacheProvider()
                .AddUI();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SampleDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            app.UseKledex().EnsureDomainDbCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
