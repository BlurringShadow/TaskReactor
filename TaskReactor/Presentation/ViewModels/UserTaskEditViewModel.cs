using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels.WelcomePage;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(NavigationService), typeof(WelcomePageViewModel))]
        public INavigationService NavigationService { get; set; }

        [NotNull, ShareVariable(nameof(TaskModel), typeof(UserTaskModel))]
        public UserTaskModel TaskModel { get; set; }

        [NotNull] readonly IUserTaskService _service;

        [ImportingConstructor,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
        public UserTaskEditViewModel([NotNull] CompositionContainer container, [NotNull] IUserTaskService service) : 
            base(container) => _service = service;

        public void Confirm()
        {
            _service.Update(TaskModel);
            NavigationService.GoBack();
        }

        public void Cancel() => NavigationService.GoBack();
    }
}