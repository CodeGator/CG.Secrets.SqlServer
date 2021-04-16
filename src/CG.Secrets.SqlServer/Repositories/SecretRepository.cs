using CG.Business.Repositories;
using CG.Linq.EFCore;
using CG.Linq.EFCore.Repositories;
using CG.Secrets.Models;
using CG.Secrets.Repositories;
using CG.Secrets.SqlServer.Repositories.Options;
using CG.Validations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Secret = CG.Secrets.Models.Secret;

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

        /// <summary>
        /// This property contains a reference to a data protection provider.
        /// </summary>
        protected IDataProtectionProvider DataProtectionProvider { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SecretRepository"/>
        /// class.
        /// </summary>
        public SecretRepository(
            IOptions<SecretRepositoryOptions> options,
            DbContextFactory<SecretDbContext> factory,
            IDataProtectionProvider dataProtectionProvider
            ) : base(options, factory)
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(dataProtectionProvider, nameof(dataProtectionProvider));

            // Save the references.
            DataProtectionProvider = dataProtectionProvider;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc/>
        public virtual Task<Secret> GetByNameAsync(
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

                // Create a data protector.
                var dataProtector = DataProtectionProvider.CreateProtector(
                    nameof(CG.Secrets)
                    );

                // Unprotect the value of the secret.
                model.Value = dataProtector.Unprotect(
                    model.Value
                    );

                // Return the results.
                return Task.FromResult(model);
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

                // Create a data protector.
                var dataProtector = DataProtectionProvider.CreateProtector(
                    nameof(CG.Secrets)
                    );

                // Protect the value of the secret.
                var protectedValue = dataProtector.Unprotect(
                    value
                    );

                // Create a model.
                var model = new Secret()
                {
                     Key = name,
                     Value = protectedValue
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
