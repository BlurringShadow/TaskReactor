﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using ApplicationDomain.DataModel;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class UserTaskEditViewModel : ScreenViewModel
    {
        [NotNull] private readonly INavigationService _navigationService;
        [NotNull] public UserTaskModel TaskModel { get; }

        [ImportingConstructor]
        public UserTaskEditViewModel(
            [NotNull] CompositionContainer container, 
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts,
            [NotNull, ShareVariable(nameof(TaskModel), typeof(UserTaskModel))]
            UserTaskModel taskModel,
            [NotNull, ShareVariable(nameof(_navigationService), typeof(WelcomePageViewModel))]
            INavigationService navigationService
        ) : base(container, variableParts)
        {
            _navigationService = navigationService;
            TaskModel = taskModel;
        }

        public void Confirm()
        {
            this.ShareWithName(TaskModel, nameof(GoalEditViewModel.CurrentUserTask));
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}