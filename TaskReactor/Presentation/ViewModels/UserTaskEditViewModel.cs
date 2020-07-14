using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Presentation.ViewModels.UserProfile.Overview;
using Presentation.Views.UserProfile.Overview;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(UserOverviewViewModel))] 
        public INavigationService NavigationService { get; set; }

        [NotNull] UserTaskModel _taskModel;

        [NotNull, ShareVariable(nameof(TaskModel), typeof(UserOverviewViewModel))] 
        public UserTaskModel TaskModel { get => _taskModel; set => Set(ref _taskModel, value); }

        [NotNull] IUserTaskService _service;

        [NotNull, Import] public IUserTaskService Service
        {
            get => _service;
            set
            {
                value.NotifyAction = model =>
                    _ = new UserTaskNotificationViewModel(new NotificationManager(), model!).ShowAsync();
                _service = value;
            }
        }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserTaskEditViewModel([NotNull] IocContainer container) : base(container)
        {
        }

        public async Task Confirm()
        {
            Service.Update(TaskModel);
            await Service.DbSync();

            NavigationService.GoBack();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}