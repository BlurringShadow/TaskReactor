using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Presentation.Utilities
{
    public static class EntityTypeBuilderExtensions
    {
        [NotNull]
        public static EntityTypeBuilder UseConverter(
            [NotNull] this EntityTypeBuilder builder,
            [NotNull] ValueConverter converter
        )
        {
            foreach (var property in builder.Metadata!.ClrType!.GetProperties()
                .Where(p => p.PropertyType == converter.ModelClrType))
                builder.Property(property!.Name)!.HasConversion(converter);

            return builder;
        }
    }
}