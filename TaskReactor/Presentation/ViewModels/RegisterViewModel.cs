using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class RegisterViewModel : ScreenViewModel
    {
        [NotNull] private readonly IUserService _userService;

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RegisteredId { get; set; }


        [ImportingConstructor]
        public RegisterViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts,
            [NotNull] IUserService userService
        ) : base(container, variableParts) => _userService = userService;

        public bool CanRegister => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);


        public async Task Register()
        {
            var user = new UserModel {Name = UserName!, Password = Password!};
            _userService.Register(user);

            await _userService.DbSync();

            RegisteredId = $"ע��ɹ���IdΪ{user.Identity}";
        }
    }
}