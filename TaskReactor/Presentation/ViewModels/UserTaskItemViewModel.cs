#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 09
// Time: 上午 11:49

#endregion

using System;
using System.Globalization;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public sealed class UserTaskItemViewModel : Screen
    {
        [NotNull] public UserTaskModel Model { get; set; }

        [NotNull] public string TaskTitle => Model.Title;

        [NotNull] public string TaskStartTime => Model.StartTime.ToString(CultureInfo.CurrentCulture);

        public event Action<UserTaskModel> OnClickEvent;

        public UserTaskItemViewModel([NotNull] UserTaskModel model) => Model = model;

        public void OnClick() => OnClickEvent?.Invoke(Model);
    }
}