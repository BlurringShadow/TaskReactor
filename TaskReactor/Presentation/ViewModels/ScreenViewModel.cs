using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public abstract class ScreenViewModel : Screen, IViewModel
    {
        public ArgsHelper ArgsHelper { get; }

        public Type InstanceType { get; }

        protected ScreenViewModel([NotNull] ArgsHelper argsArgsHelper, bool includeNonPublic = false)
        {
            ArgsHelper = argsArgsHelper;
            InstanceType = this.Initialize(includeNonPublic);
        }
    }
}