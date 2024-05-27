using Hackathon_2024_INFISOFTWARE.DataAccessLayer.DbContext;
using Hackathon_2024_INFISOFTWARE.Services.Implementations;
using Hackathon_2024_INFISOFTWARE.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Hackathon_2024_INFISOFTWARE.WebApi.Configurations
{
    public static class ServicesConfig
    {
        /// <summary>
        /// Enregistre les services métier.
        /// </summary>
        public static void RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IEmailService, EmailService>();

        }

        /// <summary>
        /// Enregistre les services utilitaires pour l'authentification.
        /// </summary>
        public static void RegisterUtilityServices(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.Configure<MongoDbSettings>(options =>
            {
                var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                configuration.GetSection("DATABASE").Bind(options);
            });

            // Enregistrer IMongoDatabase dans le conteneur d'injection de dépendances
            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });
        }

        /// <summary>
        /// Regroupe tous les enregistrements de services.
        /// </summary>
        /// <param name="services">La collection de services.</param>
        public static void RegisterServices(this IServiceCollection services)
        {
            RegisterBusinessServices(services);
            RegisterUtilityServices(services);
        }
    }
}
