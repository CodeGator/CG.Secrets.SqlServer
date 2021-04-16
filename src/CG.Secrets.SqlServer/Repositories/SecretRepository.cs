using CG.Business.Repositories;
using CG.Linq.EFCore;
using CG.Linq.EFCore.Repositories;
using CG.Secrets.Models;
using CG.Secrets.Repositories;
using CG.Secrets.SqlServer.Repositories.Options;
using CG.Validations;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CG.Secrets.SqlServer.Repositories
{
    /// <summary>
    /// This class is an Azure implementation of the <see cref="ISecretRepository"/>
    /// interface.
    /// </summary>
    public class SecretRepository :
        EFCoreRepositoryBase<SecretDbContext, IOptions<SecretRepositoryOptions>>,
        ISecretRepository
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SecretRepository"/>
        /// class.
        /// </summary>
        /// repository.</param>
        public SecretRepository(
            IOptions<SecretRepositoryOptions> options,
            DbContextFactory<SecretDbContext> factory
            ) : base(options, factory)
        {
            
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public virtual async Task<Secret> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                // Validate the parameters before attempting to use them.
                Guard.Instance().ThrowIfNullOrEmpty(name, nameof(name));

                // Create a context.
                var context = Factory.Create();

                // Defer to the data-context.
                var model = context.Secrets.FirstOrDefault(
                    x => x.Name == name
                    );

                // Return the results.
                return model;
            }
            catch (Exception ex)
            {
                // Provide better context for the error.
                throw new RepositoryException(
                    message: $"Failed to query the value of a secret, by name!",
                    innerException: ex
                    ).SetCallerInfo()
                     .SetOriginator(nameof(SecretRepository))
                     .SetDateTime();
            }
        }

        // *******************************************************************
        
        /// <inheritdoc/>
        public virtual async Task<Secret> SetByNameAsync(
            string name,
            string value,
            CancellationToken cancellationToken = default
            )
        {
            try
            {
                // Validate the parameters before attempting to use them.
                Guard.Instance().ThrowIfNullOrEmpty(name, nameof(name));

                // Create a context.
                var context = Factory.Create();

                // Create a model.
                var model = new Secret()
                {
                     Key = name,
                     Value = value 
                };

                // Defer to the data-context.
                var entity = await context.Secrets.AddAsync(
                    model,
                    cancellationToken
                    ).ConfigureAwait(false);

                // Return the results.
                return entity.Entity;
            }
            catch (Exception ex)
            {
                // Provide better context for the error.
                throw new RepositoryException(
                    message: $"Failed to set the value for a secret, by name!",
                    innerException: ex
                    ).SetCallerInfo()
                     .SetOriginator(nameof(SecretRepository))
                     .SetDateTime();
            }
        }

        #endregion
    }
}
