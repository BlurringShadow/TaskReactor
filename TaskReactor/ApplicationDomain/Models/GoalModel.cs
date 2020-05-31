using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.Models
{
    public sealed class GoalModel : ScheduleModel<Goal>
    {
        [NotNull] public UserTaskModel FromTask
        {
            get => new UserTaskModel(_dataBaseModel.FromTask);
            set
            {
                _dataBaseModel.FromTask = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }

        internal GoalModel([NotNull] Goal item) : base(item)
        {
        }
    }
}