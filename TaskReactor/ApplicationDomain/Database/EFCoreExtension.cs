using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationDomain.Database
{
    public static class EFCoreExtension
    {
        public static IEntityType FindEntityType<TEntity>(
            [NotNull] this IModel model
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

        public static string GetEntityTableName<TEntity>([NotNull] this DbContext context) where TEntity : class =>
            context.Model!.FindEntityType<TEntity>()!.GetTableName();

        [NotNull]
        public static string TruncateSqlStr([NotNull] string tableName) => $"TRUNCATE TABLE {tableName}";

        public static int TruncateTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        ) => tableNames.Sum(tableName => context.Database!.ExecuteSqlRaw(TruncateSqlStr(tableName)));

        public static int TruncateTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => context.TruncateTable((IEnumerable<string>)tableNames);

        public static async Task<int> TruncateTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        ) => await Task.Run(
            async () => (await Task.WhenAll(
                tableNames
                    .Select(tableName => context.Database!.ExecuteSqlRawAsync(TruncateSqlStr(tableName))!)
                    .ToList()
            )).Sum()
        );

        public static async Task<int> TruncateTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => await context.TruncateTableAsync((IEnumerable<string>)tableNames);

        public static async Task<int> TruncateTableFromDbSetAsync<TEntity>(
            [NotNull] this DbContext context
        ) where TEntity : class => await context.TruncateTableAsync(context.GetEntityTableName<TEntity>()!);

        public static int TruncateTableFromDbSet<TEntity>(
            [NotNull] this DbContext context
        ) where TEntity : class => context.TruncateTable(context.GetEntityTableName<TEntity>()!);

        [NotNull]
        public static string DeleteTableSqlStr(string tableName) => $"DELETE FROM {tableName}";

        public static int DeleteTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        ) => tableNames.Sum(tableName => context.Database!.ExecuteSqlRaw(DeleteTableSqlStr(tableName)));

        public static int DeleteTable(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => context.DeleteTable((IEnumerable<string>)tableNames);

        public static async Task<int> DeleteTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] IEnumerable<string> tableNames
        ) => await Task.Run(
            async () => (await Task.WhenAll(
                tableNames
                    .Select(tableName => context.Database!.ExecuteSqlRawAsync(DeleteTableSqlStr(tableName))!)
                    .ToList()
            )).Sum()
        );

        public static async Task<int> DeleteTableAsync(
            [NotNull] this DbContext context,
            [NotNull, ItemNotNull] params string[] tableNames
        ) => await context.DeleteTableAsync((IEnumerable<string>)tableNames);

        public static async Task<int> DeleteTableFromDbSetAsync<TEntity>([NotNull] this DbContext context)
            where TEntity : class =>
            await context.DeleteTableAsync(context.GetEntityTableName<TEntity>()!);

        public static int DeleteTableFromDbSet<TEntity>([NotNull] this DbContext context) where TEntity : class =>
            context.DeleteTable(context.GetEntityTableName<TEntity>()!);

        public static async Task<int> TransactionAsync<TDbContext>(
            [NotNull] this TDbContext context,
            [NotNull] Action<TDbContext> action,
            CancellationToken token
        ) where TDbContext : DbContext => await Task.Run(
            () =>
            {
                lock(context)
                {
                    using var transaction = context.Database!.BeginTransaction();

                    action(context);

                    var i = context.SaveChanges();
                    transaction!.Commit();
                    return i;
                }
            },
            token
        );

        #endregion
    }
}