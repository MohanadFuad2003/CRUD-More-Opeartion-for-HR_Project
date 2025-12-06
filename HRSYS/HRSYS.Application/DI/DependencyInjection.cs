using HRSYS.Application.Interfaces;
using HRSYS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HRSYS.Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            return services;
        }
    }
}
