using CG.Linq.EFCore;
using CG.Secrets.Repositories;
using CG.Secrets.SqlServer.Repositories;
using CG.Secrets.SqlServer.Repositories.Options;
using CG.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CG.Secrets.SqlServer
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type, for the cg.secrets.sqlserver library.
    /// </summary>
    public static partial class SecretsServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method adds SQL Server repositories for the CG.Secrets library.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for
        /// the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <param name="serviceLifetime">The service lifetime to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> parameter,
        /// for chaining calls together.</returns>
        public static IServiceCollection AddSqlServerRepositories(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Register the EFCORE options.
            serviceCollection.ConfigureOptions<SecretRepositoryOptions>(
                configuration,
                out var repositoryOptions
                );

            // Register the data-context.
            serviceCollection.AddTransient<SecretDbContext>(serviceProvider =>
            {
                // Get the options from the DI container.
                var options = serviceProvider.GetRequiredService<IOptions<SecretRepositoryOptions>>();

                // Create the options builder.
                var builder = new DbContextOptionsBuilder<SecretDbContext>();

                // Configure the options.
                builder.UseSqlServer(options.Value.ConnectionString);

                // Create the data-context.
                var context = new SecretDbContext(builder.Options);

                // Return the data-context.
                return context;
            });

            // Register the data-context factory.
            serviceCollection.Add<DbContextFactory<SecretDbContext>>(serviceLifetime);

            // Register the repositories.
            serviceCollection.Add<ISecretRepository, SecretRepository>(serviceLifetime);

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
