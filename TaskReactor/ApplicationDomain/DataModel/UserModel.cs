using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public class UserModel : Model<User>
    {
        public UserModel()
        {
        }

        internal UserModel([NotNull] User dataBaseModel) : base(dataBaseModel)
        {
        }

        [NotNull] public string Name
        {
            get => _dataBaseModel.Name;
            set
            {
                _dataBaseModel.Name = value;
                NotifyOfPropertyChange();
            }
        }

        [NotNull] public string Password
        {
            get => _dataBaseModel.Password;
            set
            {
                _dataBaseModel.Password = value;
                NotifyOfPropertyChange();
            }
        }
    }
}