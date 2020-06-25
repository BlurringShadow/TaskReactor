using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Navigation;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull] GoalModel _goalModel;

        [NotNull, ShareVariable(nameof(GoalModel), typeof(UserProfileViewModel))]
        public GoalModel GoalModel { get => _goalModel; set => Set(ref _goalModel, value); }

        [NotNull, ShareVariable(nameof(GoalModel), typeof(NavigationService))]
        public NavigationService NavigationService { get; set; }

        [NotNull] readonly IGoalService _goalService;

        [ImportingConstructor]
        public GoalEditViewModel([NotNull] CompositionContainer container, [NotNull] IGoalService goalService) :
            base(container) => _goalService = goalService;

        public void Confirm()
        {
            // TODO use goal service to implement data operation

            NavigationService.GoBack();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}