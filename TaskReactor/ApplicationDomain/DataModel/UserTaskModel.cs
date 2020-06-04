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

        [NotNull] public UserModel OwnerUserModel => new UserModel(_dataBaseModel.OwnerUser);
    }
}