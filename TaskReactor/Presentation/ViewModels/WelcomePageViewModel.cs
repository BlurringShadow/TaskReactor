using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Threading;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;
using MahApps.Metro.Controls;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class WelcomePageViewModel : ConductorOneActiveViewModel<ScreenViewModel>
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [NotNull] private readonly INavigationService _navigationService;

        [ImportingConstructor]
        public WelcomePageViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts,
            [NotNull, ShareVariable(nameof(_navigationService), typeof(MainScreenViewModel))]
            INavigationService navigationService
        ) : base(container, variableParts)
        {
            DisplayName = "Welcome";
            _navigationService = navigationService;
            this.ShareWithName(_navigationService, nameof(_navigationService));
            Transition = false;
        }

        public bool Transition
        {
            get => ActiveItem!.InstanceType == typeof(RegisterViewModel);
            set => ActiveItem = value ?
                Container.GetExportedValue<RegisterViewModel>() :
                (ScreenViewModel)Container.GetExportedValue<LogInViewModel>();
        }
    }
}