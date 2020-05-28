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
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class MainScreenViewModel : ScreenViewModel
    {
        [NotNull] public INavigationService NavigationService { get; private set; }

        [ImportingConstructor]
        public MainScreenViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts
        ) : base(container, variableParts) => DisplayName = "Task Reactor";

        /// <summary>
        /// Initialize the frame navigation service
        /// </summary>
        /// <param name="frame"></param>
        public void RegisterFrame([NotNull] Frame frame)
        {
            NavigationService = new FrameAdapter(frame);
            this.Share(NavigationService);
        }

        public void Navigate() => NavigationService.NavigateToViewModel<WelcomePageViewModel>();
    }
}