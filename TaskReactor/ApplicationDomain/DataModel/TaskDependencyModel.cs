using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public sealed class TaskDependencyModel : Model<TaskDependency>
    {
        [NotNull] public UserTaskModel Target
        {
            get => new UserTaskModel(_dataBaseModel.Target);
            set
            {
                _dataBaseModel.Target = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }

        [NotNull] public UserTaskModel Dependency
        {
            get => new UserTaskModel(_dataBaseModel.Dependency);
            set
            {
                _dataBaseModel.Dependency = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }
    }
}