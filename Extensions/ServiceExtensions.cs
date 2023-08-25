using Infrastructure.Contracts;
using Infrastructure.Repository;
using LeaveManagmentSystemAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagmentSystemAPI.Extensions
{
    public static class ServiceExtensions
    {
        //public static void ConfigureIISIntegration(this IServiceCollection services) =>
        //    services.Configure<IISOptions>(options =>
        //    {

        //    });
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("Domain")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) { 
           services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
           services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
           services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        }

    }
}
