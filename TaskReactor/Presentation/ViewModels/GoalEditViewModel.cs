using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using Data.Database.Entity;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Presentation.ViewModels.UserProfile;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull] GoalModel _goalModel;

        [NotNull, ShareVariable(nameof(GoalModel), typeof(UserProfileViewModel))]
        public GoalModel GoalModel { get => _goalModel; set => Set(ref _goalModel, value); }

        [NotNull, ShareVariable(nameof(NavigationService), typeof(UserProfileViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull] public IntervalKindListViewModel IntervalKindListViewModel { get; } = new IntervalKindListViewModel();

        public double Duration
        {
            set => GoalModel.DurationOfOneTime = TimeSpan.FromDays(value);
            get => GoalModel.DurationOfOneTime.TotalDays;
        }

        [NotNull, Import] public IGoalService Service { get; set; }

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GoalEditViewModel([NotNull] IocContainer container) : base(container) =>
            Service.NotifyAction = model =>
                _ = new GoalNotificationViewModel(new NotificationManager(), model!).ShowAsync();

        public async Task Confirm()
        {
            GoalModel.Interval.Kind = (IntervalKind)IntervalKindListViewModel.Selected;
            Service.Update(GoalModel);
            await Service.DbSync();

            NavigationService.GoBack();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}