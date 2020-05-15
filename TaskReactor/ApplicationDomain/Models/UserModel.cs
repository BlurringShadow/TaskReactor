using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;
using Utilities;

namespace ApplicationDomain.Models
{
    public class UserModel : Model<User>
    {
        public UserModel([NotNull] User dataBaseModel) : base(dataBaseModel)
        {
        }
    }
}