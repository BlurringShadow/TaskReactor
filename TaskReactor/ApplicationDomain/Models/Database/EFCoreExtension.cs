using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationDomain.Models.Database
{
    public static class EFCoreExtension
    {
        public static IEntityType FindEntityType<TEntity>(
            [NotNull] this Microsoft.EntityFrameworkCore.Metadata.IModel model
        ) where TEntity : class => model.FindEntityType(typeof(TEntity));

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

        #region DbContext Extension

        public static void TruncateTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        )
        {
            foreach (var tableName in tableNames) context.Database!.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
        }

        public static void TruncateTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => context.TruncateTable((IEnumerable<string>)tableNames);

        public static async Task TruncateTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        )
        {
            foreach (var tableName in tableNames)
                await context.Database!.ExecuteSqlRawAsync($"TRUNCATE TABLE {tableName}")!;
        }

        public static async Task TruncateTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => await context.TruncateTableAsync((IEnumerable<string>)tableNames);

        public static async Task TruncateTableFromDbSetAsync<TEntity>(
            [NotNull] this DbContext context
        ) where TEntity : class =>
            await context.TruncateTableAsync(context.Model!.FindEntityType<TEntity>()!.GetTableName()!)!;

        public static void TruncateTableFromDbSet<TEntity>(
            [NotNull] this DbContext context
        ) where TEntity : class => context.TruncateTable(context.Model!.FindEntityType<TEntity>()!.GetTableName()!);

        public static void DeleteTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        )
        {
            foreach (var tableName in tableNames) context.Database!.ExecuteSqlRaw($"DELETE FROM {tableName}");
        }

        public static void DeleteTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => context.DeleteTable((IEnumerable<string>)tableNames);

        public static async Task DeleteTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        )
        {
            foreach (var tableName in tableNames)
                await context.Database!.ExecuteSqlRawAsync($"DELETE FROM {tableName}")!;
        }

        public static async Task DeleteTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => await context.DeleteTableAsync((IEnumerable<string>)tableNames);

        public static async Task DeleteTableFromDbSetAsync<TEntity>([NotNull] this DbContext context)
            where TEntity : class =>
            await context.DeleteTableAsync(context.Model!.FindEntityType<TEntity>()!.GetTableName()!)!;

        public static void DeleteTableFromDbSet<TEntity>([NotNull] this DbContext context) where TEntity : class =>
            context.DeleteTable(context.Model!.FindEntityType<TEntity>()!.GetTableName()!);

        #endregion
    }
}