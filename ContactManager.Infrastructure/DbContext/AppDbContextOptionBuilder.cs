using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace ContactManager.Infrastructure.DbContext
{
    internal class AppDbContextOptionBuilder : IDbContextOptionsBuilderInfrastructure
    {

        public AppDbContextOptionBuilder(DbContextOptionsBuilder optionsBuilder)
        {

        }
        void IDbContextOptionsBuilderInfrastructure.AddOrUpdateExtension<TExtension>(TExtension extension)
        {
            //if (extension == null)
            //{
            //    throw new ArgumentNullException(nameof(extension));
            //}

            //// Get the existing extensions
            //var existingExtension = _optionsBuilder.Options.FindExtension<TExtension>();

            //if (existingExtension == null)
            //{
            //    // If the extension does not exist, add it
            //    ((IDbContextOptionsBuilderInfrastructure)_optionsBuilder).AddOrUpdateExtension(extension);
            //}
            //else
            //{
            //    // If the extension exists, replace it with the new one
            //    var newExtension = MergeExtensions(existingExtension, extension);
            //    ((IDbContextOptionsBuilderInfrastructure)_optionsBuilder).AddOrUpdateExtension(newExtension);
            //}
        }
    }
}
