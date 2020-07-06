using System.ComponentModel.Composition;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using ApplicationDomain.DataModel;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class GoalNotificationViewModel : NotificationViewModel
    {
        [NotNull] private GoalModel _goalModel;

        [NotNull] public GoalModel GoalModel
        {
            get => _goalModel;
            set => Set(ref _goalModel, value);
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public GoalNotificationViewModel([NotNull] INotificationManager manager, [NotNull] GoalModel goalModel) : 
            base(manager) => GoalModel = goalModel;
    }
}