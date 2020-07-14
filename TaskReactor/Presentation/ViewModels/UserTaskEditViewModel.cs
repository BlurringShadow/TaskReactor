using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Presentation.ViewModels.UserProfile;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(UserProfileViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, ShareVariable(nameof(TaskModel), typeof(UserProfileViewModel))]
        public UserTaskModel TaskModel { get; set; }

        [NotNull, Import] public IUserTaskService Service { get; set; }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserTaskEditViewModel([NotNull] IocContainer container) : base(container) =>
            Service.NotifyAction = model =>
                _ = new UserTaskNotificationViewModel(new NotificationManager(), model!).ShowAsync();

        public async Task Confirm()
        {
            Service.Update(TaskModel);
            NavigationService.GoBack();
            await Service.DbSync();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}