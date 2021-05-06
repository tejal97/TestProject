using TestApplication.Service.Student;
using Microsoft.Extensions.DependencyInjection;
using TestApplication.Data.Service.Mail;

namespace TestApplication.Extentions
{
    public static class DependencyInjectorHelperExtension
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IMailService, MailService>();
            return services;
        }
    }
}
