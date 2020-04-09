using System.Windows.Controls;
using Caliburn.Micro;

namespace TaskReactor.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly SimpleContainer _container;

        private INavigationService _navigationService;

        public MainViewModel(SimpleContainer container) => _container = container;

        public void RegisterFrame(Frame frame)
        {
            _navigationService = new FrameAdapter(frame);

            _container.Instance(_navigationService);

            _navigationService.NavigateToViewModel<WelcomePageViewModel>();
        }
    }
}