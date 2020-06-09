using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserProfileViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(WelcomePageViewModel))]
        public UserModel CurrentUser { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public BindableCollection<UserTaskItemViewModel> UserTaskItems { get; set; }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserProfileViewModel([NotNull] CompositionContainer container) : base(container)
        {
        }

        /// <summary>
        /// TODO: add task list to select
        /// </summary>
        public void AddTask()
        {
            this.ShareWithName(NavigationService, nameof(NavigationService));
            NavigationService.NavigateToViewModel<GoalEditViewModel>();
        }
    }
}