using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public sealed class TaskDependencyModel : Model<TaskDependency>
    {
        public TaskDependencyModel()
        {
        }

        internal TaskDependencyModel([NotNull] TaskDependency taskDependency) : base(taskDependency)
        {
        }

        [NotNull] public UserTaskModel Target
        {
            get => new UserTaskModel(_dataBaseModel.Target!);
            set
            {
                _dataBaseModel.Target = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }

        [NotNull] public UserTaskModel Dependency
        {
            get => new UserTaskModel(_dataBaseModel.Dependency!);
            set
            {
                _dataBaseModel.Dependency = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }
    }
}