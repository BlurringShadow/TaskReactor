using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, ShareVariable(nameof(TaskModel), typeof(UserTaskModel))]
        public UserTaskModel TaskModel { get; set; }

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserTaskEditViewModel([NotNull] CompositionContainer container) : base(container)
        {
        }

        public void Confirm()
        {
            this.ShareWithName(TaskModel, nameof(GoalEditViewModel.CurrentUserTask));
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}