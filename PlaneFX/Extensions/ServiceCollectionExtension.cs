using PlaneFX.Interfaces;

namespace PlaneFX.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var type = typeof(IService);
            var impl = type.Assembly.GetTypes()
                .Where(t => t.IsClass && type.IsAssignableFrom(t));

            foreach (var service in impl)
                services.AddTransient(service);

            return services;
        }
    }
}