using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using Data.Database.Entity;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Presentation.ViewModels.UserProfile.Overview;
using Presentation.Views.UserProfile.Overview;
using Utilities;

namespace Presentation.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull] GoalModel _goalModel;

        [NotNull, ShareVariable(nameof(GoalModel), typeof(UserOverviewViewModel))] 
        public GoalModel GoalModel { get => _goalModel; set => Set(ref _goalModel, value); }

        [NotNull, ShareVariable(nameof(NavigationService), typeof(UserOverviewViewModel))] 
        public INavigationService NavigationService { get; set; }

        [NotNull] public IntervalKindListViewModel IntervalKindListViewModel { get; } = new IntervalKindListViewModel();

        public double Duration
        {
            set => GoalModel.DurationOfOneTime = TimeSpan.FromDays(value);
            get => GoalModel.DurationOfOneTime.TotalDays;
        }

        [NotNull] IGoalService _service;

        [NotNull, Import] public IGoalService Service
        {
            get => _service;
            set
            {
                value.NotifyAction = model =>
                    _ = new GoalNotificationViewModel(new NotificationManager(), model!).ShowAsync();
                _service = value;
            }
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GoalEditViewModel([NotNull] IocContainer container) : base(container)
        {
        }

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