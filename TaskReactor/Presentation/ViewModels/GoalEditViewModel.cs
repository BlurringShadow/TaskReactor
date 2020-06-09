using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ApplicationDomain.DataModel;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    [Export, System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class GoalEditViewModel : ScreenViewModel
    {
        [NotNull, ShareVariable(nameof(CurrentUserTask), typeof(UserTaskModel))]
        public UserTaskModel CurrentUserTask
        {
            get => GoalModel.FromTask;
            set => GoalModel.FromTask = value;
        }

        [NotNull] public GoalModel GoalModel { get; }

        [ImportingConstructor]
        public GoalEditViewModel([NotNull] CompositionContainer container) : base(container) => GoalModel = new GoalModel();

        public void Confirm() => throw new NotImplementedException();

        public void Cancel() => throw new NotImplementedException();
    }
}