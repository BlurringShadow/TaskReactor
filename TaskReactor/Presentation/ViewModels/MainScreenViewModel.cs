using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Windows.Controls;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class MainScreenViewModel : ScreenViewModel
    {
        [NotNull] private INavigationService _navigationService;

        [ImportingConstructor]
        // ReSharper disable once NotNullMemberIsNotInitialized
        public MainScreenViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts
        ) : base(container, variableParts) => DisplayName = "Task Reactor";

        /// <summary> Initialize the frame navigation service </summary>
        public void RegisterFrame([NotNull] Frame frame)
        {
            _navigationService = new FrameAdapter(frame);
            this.ShareWithName(_navigationService, nameof(_navigationService));
        }

        public void Navigate() => _navigationService.NavigateToViewModel<WelcomePageViewModel>();
    }
}