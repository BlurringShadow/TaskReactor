using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class RegisterViewModel : ScreenViewModel
    {
        [NotNull] readonly IUserService _userService;

        string _userName;

        public string UserName { get => _userName; set => Set(ref _userName, value); }

        string _password;

        public string Password { get => _password; set => Set(ref _password, value); }

        string _reInputPassword;

        public string ReInputPassword { get => _reInputPassword; set => Set(ref _reInputPassword, value); }

        string _registeredId;

        public string RegisteredId { get => _registeredId; private set => Set(ref _registeredId, value); }


        [ImportingConstructor]
        public RegisterViewModel([NotNull] CompositionContainer container, [NotNull] IUserService userService) :
            base(container) => _userService = userService;

        public bool CanRegister => !string.IsNullOrEmpty(UserName) &&
                                   !string.IsNullOrEmpty(Password) &&
                                   ReInputPassword == Password;

        public async Task Register()
        {
            var user = new UserModel { Name = UserName!, Password = Password! };
            _userService.Register(user);

            await _userService.DbSync();

            RegisteredId = $"注册成功，账户为{user.Identity}";
        }
    }
}