using Caliburn.Micro;
using JetBrains.Annotations;

namespace Utilities
{
    public abstract class Model<TDataBaseModel> : PropertyChangedBase, IModel where TDataBaseModel : IDataBaseModel
    {
        [NotNull] protected readonly TDataBaseModel _dataBaseModel;

        protected Model([NotNull] TDataBaseModel dataBaseModel) => _dataBaseModel = dataBaseModel;
    }
}