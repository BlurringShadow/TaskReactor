using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

namespace Presentation.Views
{
    /// <summary>
    /// Hard copy of the <see cref="MetroNavigationWindow"/>
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleNullReferenceException"),
     SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public sealed partial class MainScreenView : IUriContext
    {
        public MainScreenView()
        {
            InitializeComponent();

            Loaded += MetroNavigationWindow_Loaded;
            Closing += MetroNavigationWindow_Closing;
        }

        void MetroNavigationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PartFrame.Navigated += PART_Frame_Navigated;
            PartFrame.Navigating += PART_Frame_Navigating;
            PartFrame.NavigationFailed += PART_Frame_NavigationFailed;
            PartFrame.NavigationProgress += PART_Frame_NavigationProgress;
            PartFrame.NavigationStopped += PART_Frame_NavigationStopped;
            PartFrame.LoadCompleted += PART_Frame_LoadCompleted;
            PartFrame.FragmentNavigation += PART_Frame_FragmentNavigation;

            PartBackButton.Click += PART_BackButton_Click;
            PartForwardButton.Click += PART_ForwardButton_Click;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoForward)
                GoForward();
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e) =>
            FragmentNavigation?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_LoadCompleted(object sender, NavigationEventArgs e) =>
            LoadCompleted?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationStopped(object sender, NavigationEventArgs e) =>
            NavigationStopped?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationProgress(object sender, NavigationProgressEventArgs e) =>
            NavigationProgress?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) =>
            NavigationFailed?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e) =>
            Navigating?.Invoke(this, e);

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoBack)
                GoBack();
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void MetroNavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PartFrame.FragmentNavigation -= PART_Frame_FragmentNavigation;
            PartFrame.Navigating -= PART_Frame_Navigating;
            PartFrame.NavigationFailed -= PART_Frame_NavigationFailed;
            PartFrame.NavigationProgress -= PART_Frame_NavigationProgress;
            PartFrame.NavigationStopped -= PART_Frame_NavigationStopped;
            PartFrame.LoadCompleted -= PART_Frame_LoadCompleted;
            PartFrame.Navigated -= PART_Frame_Navigated;

            PartForwardButton.Click -= PART_ForwardButton_Click;
            PartBackButton.Click -= PART_BackButton_Click;

            Loaded -= MetroNavigationWindow_Loaded;
            Closing -= MetroNavigationWindow_Closing;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigated(object sender, NavigationEventArgs e)
        {
            PartTitle.Content = ((Page)PartFrame.Content).Title;
            (this as IUriContext).BaseUri = e.Uri;

            PageContent = PartFrame.Content;

            PartBackButton.IsEnabled = CanGoBack;

            PartForwardButton.IsEnabled = CanGoForward;

            Navigated?.Invoke(this, e);
        }

        public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register(
            nameof(OverlayContent), typeof(object), typeof(MetroNavigationWindow)
        );

        public object OverlayContent
        {
            get => GetValue(OverlayContentProperty);
            set => SetValue(OverlayContentProperty, value);
        }

        public static readonly DependencyProperty PageContentProperty = DependencyProperty.Register(
            nameof(PageContent), typeof(object), typeof(MetroNavigationWindow)
        );

        public object PageContent
        {
            get => GetValue(PageContentProperty);
            private set => SetValue(PageContentProperty, value);
        }

        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history for a NavigationWindow.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.ForwardStack"/>
        public IEnumerable ForwardStack => PartFrame.ForwardStack;

        /// <summary>
        /// Gets an IEnumerable that you use to enumerate the entries in back navigation history for a NavigationWindow.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.BackStack"/>
        public IEnumerable BackStack => PartFrame.BackStack;

        /// <summary>
        /// Gets the NavigationService that is used by this MetroNavigationWindow to provide navigation services to its content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationService"/>
        public NavigationService NavigationService => PartFrame.NavigationService;

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.CanGoBack"/>
        public bool CanGoBack => PartFrame.CanGoBack;

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in forward navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.CanGoForward"/>
        public bool CanGoForward => PartFrame.CanGoForward;

        /// <summary>
        /// Gets or sets the base uniform resource identifier (URI) of the current context.
        /// </summary>
        /// <see cref="IUriContext.BaseUri"/>
        Uri IUriContext.BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to.  
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Source"/>
        public Uri Source { get => PartFrame.Source; set => PartFrame.Source = value; }

        /// <summary>
        /// Adds an entry to back navigation history that contains a CustomContentState object.
        /// </summary>
        /// <param name="state">A CustomContentState object that represents application-defined state that is associated with a specific piece of content.</param>
        /// <see cref="System.Windows.Navigation.NavigationWindow.AddBackEntry"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public void AddBackEntry(CustomContentState state)
        {
            PartFrame.AddBackEntry(state);
        }

        /// <summary>
        /// Removes the most recent journal entry from back history.
        /// </summary>
        /// <returns>The most recent JournalEntry in back navigation history, if there is one.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.RemoveBackEntry"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public JournalEntry RemoveBackEntry()
        {
            return PartFrame.RemoveBackEntry();
        }

        /// <summary>
        /// Navigates to the most recent item in back navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.GoBack"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public void GoBack() => PartFrame.GoBack();

        /// <summary>
        /// Navigates to the most recent item in forward navigation history.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.GoForward"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public void GoForward() => PartFrame.GoForward();

        /// <summary>
        /// Navigates asynchronously to content that is contained by an object.
        /// </summary>
        /// <param name="content">An Object that contains the content to navigate to.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(object)"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(object content) => PartFrame.Navigate(content);

        /// <summary>
        /// Navigates asynchronously to content that is specified by a uniform resource identifier (URI).
        /// </summary>
        /// <param name="source">A Uri object initialized with the URI for the desired content.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigate(Uri)"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source) => PartFrame.Navigate(source);

        /// <summary>
        /// Navigates asynchronously to content that is contained by an object, and passes an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="content">An Object that contains the content to navigate to.</param>
        /// <param name="extraData">A Object that contains data to be used for processing during navigation.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="NavigationWindow.Navigate(object, object)"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(object content, object extraData)
        {
            return PartFrame.Navigate(content, extraData);
        }

        /// <summary>
        /// Navigates asynchronously to source content located at a uniform resource identifier (URI), and pass an object that contains data to be used for processing during navigation.
        /// </summary>
        /// <param name="source">A Uri object initialized with the URI for the desired content.</param>
        /// <param name="extraData">A Object that contains data to be used for processing during navigation.</param>
        /// <returns>true if a navigation is not canceled; otherwise, false.</returns>
        /// <see cref="NavigationWindow.Navigate(Uri, object)"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source, object extraData) => PartFrame.Navigate(source, extraData);

        /// <summary>
        /// Stops further downloading of content for the current navigation request.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.StopLoading"/>
        [System.Diagnostics.DebuggerNonUserCode]
        public void StopLoading() => PartFrame.StopLoading();

        /// <summary>
        /// Occurs when navigation to a content fragment begins, which occurs immediately, if the desired fragment is in the current content, or after the source XAML content has been loaded, if the desired fragment is in different content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.FragmentNavigation"/>
        public event FragmentNavigationEventHandler FragmentNavigation;

        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigating"/>
        public event NavigatingCancelEventHandler Navigating;

        /// <summary>
        /// Occurs when an error is raised while navigating to the requested content.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationFailed"/>
        public event NavigationFailedEventHandler NavigationFailed;

        /// <summary>
        /// Occurs periodically during a download to provide navigation progress information.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationProgress"/>
        public event NavigationProgressEventHandler NavigationProgress;

        /// <summary>
        /// Occurs when the StopLoading method is called, or when a new navigation is requested while a current navigation is in progre
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.NavigationStopped"/>
        public event NavigationStoppedEventHandler NavigationStopped;

        /// <summary>
        /// Occurs when the content that is being navigated to has been found, and is available from the PageContent property, although it may not have completed loading
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.Navigated"/>
        public event NavigatedEventHandler Navigated;

        /// <summary>
        /// Occurs when content that was navigated to has been loaded, parsed, and has begun rendering.
        /// </summary>
        /// <see cref="System.Windows.Navigation.NavigationWindow.LoadCompleted"/>
        public event LoadCompletedEventHandler LoadCompleted;
    }
}