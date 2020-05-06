using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Presentation.Utilities
{
    public static class EFCoreModelExtension
    {
        public static IEntityType FindEntityType<TEntity>([NotNull] this Microsoft.EntityFrameworkCore.Metadata.IModel model) where TEntity : class =>
            model.FindEntityType(typeof(TEntity));
    }
}