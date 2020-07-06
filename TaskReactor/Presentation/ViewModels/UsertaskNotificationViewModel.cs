using JetBrains.Annotations;
using Notifications.Wpf.Core;
using ApplicationDomain.DataModel;

namespace Presentation.ViewModels
{
    public sealed class UserTaskNotificationViewModel : NotificationViewModel
    {
        [NotNull] private UserTaskModel _model;

        [NotNull] public UserTaskModel Model { get => _model; set => Set(ref _model, value); }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public UserTaskNotificationViewModel([NotNull] INotificationManager manager, [NotNull] UserTaskModel model) :
            base(manager) => Model = model;
    }
}