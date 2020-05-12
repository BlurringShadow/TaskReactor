#pragma warning disable 649

using System.ComponentModel.Composition;
using ApplicationDomain.Models.DataBase.Entity;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized"),
     System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public class UserProfileViewModel : ScreenViewModel
    {
        [NotNull, Args(typeof(WelcomePageViewModel))] 
        private readonly INavigationService _navigationService;

        [Args(typeof(WelcomePageViewModel)), NotNull]
        public readonly User CurrentUser;

        [ImportingConstructor]
        public UserProfileViewModel([NotNull] ArgsHelper argsArgsHelper) : base(argsArgsHelper)
        {
        }
        
        public void AddSchedule()
        {
            this.Update<UserProfileViewModel>(_navigationService, nameof(_navigationService));
            _navigationService.NavigateToViewModel<ScheduleEditViewModel>();
        }
    }
}