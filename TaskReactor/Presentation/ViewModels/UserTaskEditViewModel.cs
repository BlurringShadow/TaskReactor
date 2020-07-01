using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile;
using Utilities;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(UserProfileViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, ShareVariable(nameof(TaskModel), typeof(UserProfileViewModel))]
        public UserTaskModel TaskModel { get; set; }

        [NotNull] readonly IUserTaskService _service;

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserTaskEditViewModel([NotNull] IocContainer container, [NotNull] IUserTaskService service) : 
            base(container) => _service = service;

        public async Task Confirm()
        {
            _service.Update(TaskModel);
            NavigationService.GoBack();
            await _service.DbSync();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}