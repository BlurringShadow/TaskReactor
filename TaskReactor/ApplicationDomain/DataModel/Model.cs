using System;
using Caliburn.Micro;
using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    /// <summary>
    /// Model data view of the entity type
    /// </summary>
    /// <typeparam name="TDataBaseModel"> Data base entity </typeparam>
    public abstract class Model<TDataBaseModel> : PropertyChangedBase, IModel
        where TDataBaseModel : class, IDatabaseModel, new()
    {
        [NotNull] protected internal readonly TDataBaseModel _dataBaseModel;

        [NotNull] public Type InstanceType { get; }

        /// <summary>
        /// Create a default model instance with  <see cref="TDataBaseModel"/> default paramless constructor.
        /// </summary>
        protected Model() : this(new TDataBaseModel())
        {
        }

        internal Model([NotNull] TDataBaseModel dataBaseModel)
        {
            _dataBaseModel = dataBaseModel;
            InstanceType = GetType();
        }
    }
}