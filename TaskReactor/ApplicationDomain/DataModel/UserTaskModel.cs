using System;
using Data.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public sealed class UserTaskModel : ItemModel<UserTask>
    {
        public UserTaskModel()
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
        }

        internal UserTaskModel([NotNull] UserTask userTask) : base(userTask)
        {
        }

        public int Identity => _dataBaseModel.Id;

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