#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 09
// Time: 下午 10:47

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation
{
    /// <summary>
    /// Modified from the implementation of <see cref="FrameAdapter"/>
    /// A basic implementation of <see cref="INavigationService" /> designed to adapt the <see cref="NavigationService" /> control.
    /// </summary>
    internal class CMNavigationService : INavigationService
    {
        [NotNull] readonly NavigationService _navigationService;
        readonly bool _treatViewAsLoaded;
        event NavigatingCancelEventHandler ExternalNavigatingHandler;

        /// <summary>
        ///   Creates an instance of <see cref="CMNavigationService" />
        /// </summary>
        /// <param name="navigationService"> The _navigationService to represent as a <see cref="INavigationService" /> . </param>
        /// <param name="treatViewAsLoaded"> Tells the _navigationService adapter to assume that the view has already been loaded by the time OnNavigated is called. This is necessary when using the TransitionFrame. </param>
        public CMNavigationService([NotNull] NavigationService navigationService, bool treatViewAsLoaded = false)
        {
            _navigationService = navigationService;
            _treatViewAsLoaded = treatViewAsLoaded;
            _navigationService.Navigating += OnNavigating;
            _navigationService.Navigated += OnNavigated;
        }

        /// <summary>
        ///   Occurs before navigation
        /// </summary>
        /// <param name="sender"> The event sender. </param>
        /// <param name="e"> The event args. </param>
        protected virtual async void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            ExternalNavigatingHandler?.Invoke(sender, e);
            if (e is null || e.Cancel || !(_navigationService.Content is FrameworkElement fe))
                return;

            if (fe.DataContext is IGuardClose guard)
            {
                // ReSharper disable once PossibleNullReferenceException
                var canClose = await guard.CanCloseAsync(CancellationToken.None);

                if (!canClose)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // If we are navigating to the same page there is no need to deactivate
            // e.g. When the app is activated with Fast Switch
            if (fe.DataContext is IDeactivate deactivator && _navigationService.CurrentSource != e.Uri)
            {
                // ReSharper disable once PossibleNullReferenceException
                await deactivator.DeactivateAsync(false, CancellationToken.None);
            }
        }

        /// <summary>
        ///   Occurs after navigation
        /// </summary>
        /// <param name="sender"> The event sender. </param>
        /// <param name="e"> The event args. </param>
        protected virtual async void OnNavigated(object sender, NavigationEventArgs e)
        {
            var content = e?.Content;
            if (content is null || e.Uri?.IsAbsoluteUri == true) return;

            ViewLocator.InitializeComponent(content);

            // ReSharper disable once PossibleNullReferenceException
            var viewModel = ViewModelLocator.LocateForView(e.Content);

            if (viewModel == null) return;

            if (!(e.Content is Page page))
                throw new ArgumentException(
                    "View '" + content.GetType().FullName + "' should inherit from Page or one of its descendents."
                );

            // ReSharper disable once AssignNullToNotNullAttribute
            if (_treatViewAsLoaded) page.SetValue(View.IsLoadedProperty, true);

            TryInjectParameters(viewModel, e.ExtraData);
            // ReSharper disable once PossibleNullReferenceException
            ViewModelBinder.Bind(viewModel, page, null);

            if (viewModel is IActivate activator) await activator.ActivateAsync()!;

            GC.Collect();
        }

        /// <summary>
        ///   Attempts to inject query string parameters from the view into the view model.
        /// </summary>
        /// <param name="viewModel"> The view model.</param>
        /// <param name="parameter"> The parameter.</param>
        void TryInjectParameters([NotNull] object viewModel, object parameter)
        {
            var viewModelType = viewModel.GetType();

            if (parameter is IDictionary<string, object> dictionaryParameter)
                foreach (var (key, value) in dictionaryParameter)
                {
                    var property = viewModelType.GetPropertyCaseInsensitive(key);
                    property?.SetValue(
                        viewModel, MessageBinder.CoerceValue(property.PropertyType, value, null), null
                    );
                }
            else
            {
                var property = viewModelType.GetPropertyCaseInsensitive("Parameter");
                property?.SetValue(viewModel, MessageBinder.CoerceValue(property.PropertyType, parameter, null), null);
            }
        }

        /// <summary>
        ///   The <see cref="Uri" /> source.
        /// </summary>
        public Uri Source { get => _navigationService.Source; set => _navigationService.Source = value; }

        /// <summary>
        ///   Indicates whether the navigator can navigate back.
        /// </summary>
        public bool CanGoBack => _navigationService.CanGoBack;

        /// <summary>
        ///   Indicates whether the navigator can navigate forward.
        /// </summary>
        public bool CanGoForward => _navigationService.CanGoForward;

        /// <summary>
        ///   The current <see cref="Uri" /> source.
        /// </summary>
        public Uri CurrentSource => _navigationService.CurrentSource;

        /// <summary>
        ///   The current content.
        /// </summary>
        public object CurrentContent => _navigationService.Content;

        /// <summary>
        ///   Stops the loading process.
        /// </summary>
        public void StopLoading() => _navigationService.StopLoading();

        /// <summary>
        ///   Navigates back.
        /// </summary>
        public void GoBack() =>
            _navigationService.GoBack();

        /// <summary>
        ///   Navigates forward.
        /// </summary>
        public void GoForward() =>
            _navigationService.GoForward();

        public void NavigateToViewModel(Type viewModel, object extraData = null) =>
            _navigationService.Navigate(
                new Uri(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    ViewLocator.DeterminePackUriFromType(
                        // ReSharper disable once PossibleNullReferenceException
                        viewModel, ViewLocator.LocateTypeForModelType(viewModel, null, null)
                    ), UriKind.Relative
                ), extraData
            );

        public void NavigateToViewModel<TViewModel>(object extraData = null) =>
            NavigateToViewModel(typeof(TViewModel), extraData);

        /// <summary>
        ///   Removes the most recent entry from the back stack.
        /// </summary>
        /// <returns> The entry that was removed. </returns>
        public JournalEntry RemoveBackEntry() =>
            _navigationService.RemoveBackEntry();

        /// <summary>
        ///   Raised after navigation.
        /// </summary>
        public event NavigatedEventHandler Navigated
        {
            add => _navigationService.Navigated += value;
            remove => _navigationService.Navigated -= value;
        }

        /// <summary>
        ///   Raised prior to navigation.
        /// </summary>
        public event NavigatingCancelEventHandler Navigating
        {
            add => ExternalNavigatingHandler += value;
            remove => ExternalNavigatingHandler -= value;
        }

        /// <summary>
        ///   Raised when navigation fails.
        /// </summary>
        public event NavigationFailedEventHandler NavigationFailed
        {
            add => _navigationService.NavigationFailed += value;
            remove => _navigationService.NavigationFailed -= value;
        }

        /// <summary>
        ///   Raised when navigation is stopped.
        /// </summary>
        public event NavigationStoppedEventHandler NavigationStopped
        {
            add => _navigationService.NavigationStopped += value;
            remove => _navigationService.NavigationStopped -= value;
        }

        /// <summary>
        ///   Raised when a fragment navigation occurs.
        /// </summary>
        public event FragmentNavigationEventHandler FragmentNavigation
        {
            add => _navigationService.FragmentNavigation += value;
            remove => _navigationService.FragmentNavigation -= value;
        }
    }
}