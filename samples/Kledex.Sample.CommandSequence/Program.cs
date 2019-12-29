using Microsoft.Extensions.DependencyInjection;
using System;
using Kledex.Extensions;
using Kledex.Sample.CommandSequence.Commands;
using Kledex.Utilities;
using Kledex.Validation.FluentValidation.Extensions;

namespace Kledex.Sample.CommandSequence
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var dispatcher = serviceProvider.GetService<IDispatcher>();

            var result = AsyncUtil.RunSync(() => dispatcher.SendAsync<string>(new SampleCommandSequence()));

            Console.WriteLine($"Final result: {result}");

            Console.ReadLine();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddKledex(typeof(Program))
                .AddFluentValidation();

            return services.BuildServiceProvider();
        }
    }
}
