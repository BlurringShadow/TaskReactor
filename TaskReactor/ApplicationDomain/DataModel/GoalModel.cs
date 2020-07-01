using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
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

        public int Identity => _dataBaseModel.Id;

        public GoalModel() => _dataBaseModel.Interval = new Interval();

        internal GoalModel([NotNull] Goal item) : base(item)
        {
        }
    }
}