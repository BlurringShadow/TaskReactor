using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Controls;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels.WelcomePage
{
    [Export]
    public sealed class RegisterViewModel : ScreenViewModel
    {
        [NotNull] readonly IUserService _userService;

        string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                Set(ref _userName, value);
                NotifyOfPropertyChange(nameof(CanRegister));
            }
        }

        string _password;

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
                NotifyOfPropertyChange(nameof(CanRegister));
            }
        }

        string _reInputPassword;

        public string ReInputPassword
        {
            get => _reInputPassword;
            set             
            {
                Set(ref _reInputPassword, value);
                NotifyOfPropertyChange(nameof(CanRegister));
            }
        }

        public bool CanRegister => !string.IsNullOrEmpty(UserName) &&
                                   !string.IsNullOrEmpty(Password) &&
                                   ReInputPassword == Password;

        string _registeredId;

        public string RegisteredId { get => _registeredId; private set => Set(ref _registeredId, value); }

        public void SetPassword([NotNull] PasswordBox value) => Password = value.Password;

        public void SetReInputPassword([NotNull] PasswordBox value) => ReInputPassword = value.Password;

        [ImportingConstructor]
        public RegisterViewModel([NotNull] IocContainer container, [NotNull] IUserService userService) :
            base(container) => _userService = userService;

        public async Task Register()
        {
            var user = new UserModel { Name = UserName!, Password = Password! };
            _userService.Register(user);

            await _userService.DbSync();

            RegisteredId = $"注册成功，账户为{user.Identity}";
        }
    }
}