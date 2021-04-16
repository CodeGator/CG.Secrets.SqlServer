using CG.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CG.Secrets.SqlServer
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="SecretDbContext"/>
    /// type.
    /// </summary>
    /// <remarks>
    /// This class contains <see cref="SecretDbContext"/> related operations that should
    /// only be called from within the <see cref="CG.Secrets.SqlServer"/> library.
    /// </remarks>
    internal static partial class SecretDbContextExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method applies seed data to the specified data-context.
        /// </summary>
        /// <param name="context">The data-context to use for the operation.</param>
        public static void ApplySeedData(
            this SecretDbContext context
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(context, nameof(context));

            // Add data to the tables.
            //context.SeedSecrets();
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method applies seed data to the Secrets table.
        /// </summary>
        /// <param name="context">The data-context to use for the operation.</param>
        private static void SeedSecrets(
            this SecretDbContext context
            )
        {
            // Don't seed an already populated table.
            if (true == context.Secrets.Any())
            {
                return;
            }

            // Add data to the table.
            context.AddRange(new Models.Secret[]
            {
                new Models.Secret()
                {
                    
                }
            }); 

            // Save the changes.
            context.SaveChanges();
        }

        #endregion
    }
}
