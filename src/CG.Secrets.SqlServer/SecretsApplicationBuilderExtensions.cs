using CG.Secrets.SqlServer.Repositories.Options;
using CG.Validations;
using Microsoft.AspNetCore.Builder;
using System;

namespace CG.Secrets.SqlServer
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IApplicationBuilder"/>
    /// type.
    /// </summary>
    public static partial class SecretsApplicationBuilderExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method wires up any startup logic required to support the 
        /// repositories and underyling SQL Server for the CG.Secrets library.
        /// </summary>
        /// <param name="applicationBuilder">The application builder to use for
        /// the operation.</param>
        /// <param name="configurationSection">The configuration section name
        /// that corresponds with the repositories.</param>
        /// <returns>The value of the <paramref name="applicationBuilder"/> parameter,
        /// for chaining calls together.</returns>
        public static IApplicationBuilder UseSqlServerRepositories(
            this IApplicationBuilder applicationBuilder,
            string configurationSection
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(applicationBuilder, nameof(applicationBuilder))
                .ThrowIfNullOrEmpty(configurationSection, nameof(configurationSection));

            // Startup EFCore.
            applicationBuilder.UseEFCore<SecretDbContext, SecretRepositoryOptions>(
                (context, wasDropped, wasMigrated) =>
                {
                    // Add seed data to the data-context.
                    context.ApplySeedData();
                });

            // Return the application builder.
            return applicationBuilder;
        }

        #endregion
    }
}
