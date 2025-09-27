using BlogWebApp.Service.FluentValidations;
using BlogWebApp.Service.Helpers.Images;
using BlogWebApp.Service.Helpers.SendEmail;
using BlogWebApp.Service.Helpers.Slug;
using BlogWebApp.Service.Services.Abstractions;
using BlogWebApp.Service.Services.Concreates;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace BlogWebApp.Service.Extensions
{
    public  static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var asembly = Assembly.GetExecutingAssembly();
     

            services.Scan(scan => scan.FromAssemblies(asembly)
            .AddClasses(publicOnly: false)
            .AsMatchingInterface()
            .WithScopedLifetime()); 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(asembly);

          


            services.AddControllersWithViews();
            services.AddDataProtection();
            services.AddFluentValidationAutoValidation() 
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<ArticleValidator>();
 
            FluentValidation.ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr"); 


            return services;

        }
    }
}
