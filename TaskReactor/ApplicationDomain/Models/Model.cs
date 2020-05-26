using ApplicationDomain.Models.Database.Entity;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace ApplicationDomain.Models
{
    /// <summary>
    /// Model view of the entity type
    /// </summary>
    /// <typeparam name="TDataBaseModel"> Data base entity </typeparam>
    public abstract class Model<TDataBaseModel> : PropertyChangedBase, IModel
        where TDataBaseModel : IDataBaseModel, new()
    {
        /// <summary>
        /// Create a default model instance with  <see cref="TDataBaseModel"/> default paramless constructor.
        /// </summary>
        protected Model() : this(new TDataBaseModel())
        {
        }

        protected Model([NotNull] TDataBaseModel dataBaseModel) => _dataBaseModel = dataBaseModel;

        [NotNull] protected internal TDataBaseModel _dataBaseModel { get; }
    }
}