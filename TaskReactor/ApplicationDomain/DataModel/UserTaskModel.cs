using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public class UserTaskModel : ItemModel<UserTask>
    {
        public UserTaskModel()
        {
        }

        internal UserTaskModel([NotNull] UserTask userTask) : base(userTask)
        {
        }

        [NotNull] public UserModel OwnerUser
        {
            get => new UserModel(_dataBaseModel.OwnerUser);
            set
            {
                _dataBaseModel.OwnerUser = value._dataBaseModel;
                NotifyOfPropertyChange();
            }
        }
    }
}