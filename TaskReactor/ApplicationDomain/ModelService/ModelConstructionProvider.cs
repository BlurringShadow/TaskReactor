using System;
using System.Reflection;
using ApplicationDomain.DataModel;
using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    static class ModelConstructionProvider<TModel, TDatabaseModel>
        where TDatabaseModel : DatabaseModel, new()
        where TModel : Model<TDatabaseModel>, new()
    {
        [NotNull] private static readonly Type _dataBaseType = typeof(TDatabaseModel);

        [NotNull] private static readonly Type _modelType = typeof(TModel);

        // ReSharper disable once StaticMemberInGenericType
        [NotNull] private static readonly ConstructorInfo _modelConstructor = _modelType.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new[] { _dataBaseType },
            null
        )!;

        [NotNull]
        public static TModel CreateModelInstance(TDatabaseModel databaseModel) =>
            databaseModel is null ? new TModel() : (TModel)_modelConstructor.Invoke(new object[] { databaseModel });
    }
}