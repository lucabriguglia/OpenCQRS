using Kledex.Bus.ServiceBus.Extensions;
using Kledex.Caching.Memory.Extensions;
using Kledex.Caching.Redis.Extensions;
using Kledex.Extensions;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Store.Cosmos.Sql;
using Kledex.Store.EF.Cosmos.Extensions;
using Kledex.UI.Extensions;
using Kledex.Validation.FluentValidation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kledex.Sample.EventSourcing
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

            services.AddDbContext<ReportingDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ReadModel")));

            services
                .AddKledex(options => 
                {
                    options.PublishEvents = true;
                    options.SaveCommandData = true;
                    options.ValidateCommands = false;
                }, typeof(Product))
                .AddCosmosStore(options =>
                {
                    options.ServiceEndpoint = Configuration.GetSection("KledexCosmos:ServerEndpoint").Value;
                    options.AuthKey = Configuration.GetSection("KledexCosmos:AuthKey").Value;
                    options.OfferThroughput = 400;
                    options.ConsistencyLevel = ConsistencyLevel.Session;
                })
                .AddServiceBus(options => {
                    options.ConnectionString = Configuration.GetConnectionString("MyMessageBus");
                })
                .AddFluentValidation()
                //.AddRedisCache(options => {
                //    options.ConnectionString = Configuration.GetConnectionString("MyRedisCache");
                //})
                .AddMemoryCache()
                .AddUI();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReportingDbContext dbContext, IOptions<DomainDbOptions> settings)
        {
            dbContext.Database.EnsureCreated();
            //app.UseKledex().EnsureCosmosDbSqlDbCreated(settings);
            app.UseKledex().EnsureCosmosDbCreated();

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
