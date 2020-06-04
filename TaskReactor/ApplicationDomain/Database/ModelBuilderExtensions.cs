using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationDomain.Database
{
    public static class ModelBuilderExtensions
    {
        [NotNull]
        public static ModelBuilder UseConverterForAllEntity(
            [NotNull] this ModelBuilder builder,
            [NotNull] ValueConverter converter
        )
        {
            foreach (var entityType in builder.Model!.GetEntityTypes()!)
                builder.Entity(entityType!.ClrType!)!.UseConverter(converter);
            return builder;
        }
    }
}