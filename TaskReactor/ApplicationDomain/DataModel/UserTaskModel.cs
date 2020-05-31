using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public class UserTaskModel : ItemModel<UserTask>
    {
        internal UserTaskModel([NotNull] UserTask userTask) : base(userTask)
        {
        }

        public override string Title
        {
            get => base.Title;
            set
            {
                _dataBaseModel.Id = (_dataBaseModel.OwnerUser.Id, value).GetHashCode();
                base.Title = value;
            }
        }

        [NotNull] public UserModel OwnerUserModel
        {
            get => new UserModel(_dataBaseModel.OwnerUser);
            set => _dataBaseModel.Id = (value._dataBaseModel.Id, Title).GetHashCode();
        }
    }
}