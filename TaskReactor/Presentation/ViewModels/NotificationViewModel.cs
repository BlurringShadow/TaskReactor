#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 24
// Time: 下午 7:03

#endregion

using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Action = System.Action;

namespace Presentation.ViewModels
{
    public abstract class NotificationViewModel : ScreenViewModel, INotificationViewModel
    {
        public INotificationManager Manager { get; }

        public event Action OnClose;

        public event Action OnClick;

        public TimeSpan Duration { get; set; }

        protected NotificationViewModel([NotNull] INotificationManager manager, [NotNull] CompositionContainer container) :
            base(container) => Manager = manager;

        public async Task ShowAsync(CancellationToken token) =>
            await Manager.ShowAsync(this, null, Duration, OnClick, OnClose, token);

        public async Task ShowAsync() => await ShowAsync(CancellationToken.None);
    }
}