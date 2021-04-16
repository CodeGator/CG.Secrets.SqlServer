using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Secrets.SqlServer.Maps
{
    /// <summary>
    /// This class represents the entity mapping for the <see cref="CG.Secrets.Models.Secret"/>
    /// object.
    /// </summary>
    /// <remarks>
    /// This class contains logic to build an EFCORE mapping between the <see cref="CG.Secrets.Models.Secret"/>
    /// entity type and the Secret.Secrets table, in the database.
    /// </remarks>
    internal class SecretMap : IEntityTypeConfiguration<CG.Secrets.Models.Secret>
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method configures the <see cref="Models.Secret"/> entity.
        /// </summary>
        /// <param name="builder">The builder to use for the operation.</param>
        public void Configure(
            EntityTypeBuilder<Models.Secret> builder
            )
        {
            builder.ToTable("Secrets", "Secret");

            /*
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Sid)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.SKey)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(e => e.IsLocked)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(50);

            builder.Property(e => e.UpdatedDate);

            builder.HasIndex(e => e.Name).IsUnique();
            */
        }

        #endregion
    }
}
