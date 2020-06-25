using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Navigation;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using JetBrains.Annotations;
using Presentation.ViewModels.UserProfile;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull] GoalModel _goalModel;

        [NotNull, ShareVariable(nameof(GoalModel), typeof(UserProfileViewModel))]
        public GoalModel GoalModel { get => _goalModel; set => Set(ref _goalModel, value); }

        [NotNull, ShareVariable(nameof(GoalModel), typeof(NavigationService))]
        public NavigationService NavigationService { get; set; }

        [NotNull] public IntervalKindListViewModel IntervalKindListViewModel { get; } = new IntervalKindListViewModel();

        public double Duration
        {
            set => GoalModel.DurationOfOneTime = TimeSpan.FromDays(value);
            get => GoalModel.DurationOfOneTime.TotalDays;
        }

        [NotNull] readonly IGoalService _goalService;

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GoalEditViewModel([NotNull] CompositionContainer container, [NotNull] IGoalService goalService) :
            base(container) => _goalService = goalService;

        public void Confirm()
        {
            GoalModel.Interval.Kind = (IntervalKind)IntervalKindListViewModel.Selected;
            _goalService.Update(GoalModel);
            NavigationService.GoBack();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}