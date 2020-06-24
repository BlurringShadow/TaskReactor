#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 下午 7:46

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Notifications.Wpf.Core;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Provide notification presentation support
    /// </summary>
    interface INotificationViewModel : IViewModel, Notifications.Wpf.Core.INotificationViewModel
    {
        /// <summary>
        /// <see cref="INotificationManager"/> support for basic functionality
        /// </summary>
        [NotNull] INotificationManager Manager { get; }

        /// <summary>
        /// Time for notification duration
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Show the notification
        /// </summary>
        Task ShowAsync(CancellationToken token);

        /// <summary>
        /// Show the notification
        /// </summary>
        Task ShowAsync();

        /// <summary>
        /// Event when notification close
        /// </summary>
        event Action OnClose;

        /// <summary>
        /// Event when notification click
        /// </summary>
        event Action OnClick;
    }
}