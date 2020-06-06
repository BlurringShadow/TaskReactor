using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using ApplicationDomain.DataModel;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull] public UserTaskModel CurrentUserTask
        {
            get => GoalModel.FromTask;
            set => GoalModel.FromTask = value;
        }

        [NotNull] public GoalModel GoalModel { get; }

        [ImportingConstructor]
        public GoalEditViewModel(
            [NotNull] CompositionContainer container,
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts,
            [NotNull, ShareVariable(nameof(CurrentUserTask), typeof(UserTaskModel))]
            UserTaskModel currentUserTask
        ) : base(container, variableParts)
        {
            GoalModel = new GoalModel();
            CurrentUserTask = currentUserTask;
        }

        public void Confirm() => throw new NotImplementedException();

        public void Cancel() => throw new NotImplementedException();
    }
}