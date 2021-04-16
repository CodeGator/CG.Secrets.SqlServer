using CG.Secrets.SqlServer.Maps;
using Microsoft.EntityFrameworkCore;
using System;

namespace CG.Secrets.SqlServer
{
    /// <summary>
    /// This class is a data-context for the CG.Secrets.SqlServer library.
    /// </summary>
    public class SecretDbContext : DbContext
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a set of <see cref="CG.Secrets.Models.Secret"/>
        /// objects.
        /// </summary>
        public virtual DbSet<CG.Secrets.Models.Secret> Secrets { get; set; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="SecretDbContext"/>
        /// class.
        /// </summary>
        /// <param name="options">The options to use with the data-context.</param>
        public SecretDbContext(
            DbContextOptions<SecretDbContext> options
            ) : base(options)
        {

        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is called to create the data model for the data-context.
        /// </summary>
        /// <param name="modelBuilder">The builder to use for the operation.</param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder
            )
        {
            // Build up the data model.
            modelBuilder.ApplyConfiguration(new SecretMap());

            // Give the base class a chance.
            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
