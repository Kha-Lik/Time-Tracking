using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TimeTracking.Common.Abstract;

namespace TimeTracking.Common.Services
{
    public static class TemplateStorageExtensions
    {
        public static IServiceCollection  RegisterTemplateServices(this  IServiceCollection services)
        {
            services.AddSingleton<IReadOnlyTemplateStorageService, TemplateStorageService>();
            services.AddSingleton<IWriteableTemplateStorageService, TemplateStorageService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            return services;
        }
    }
}