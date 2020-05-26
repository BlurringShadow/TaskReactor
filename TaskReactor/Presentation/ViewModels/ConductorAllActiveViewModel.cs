using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public abstract class ConductorAllActiveViewModel<T> : Conductor<T>.Collection.AllActive, IViewModel where T : class
    {
        public ArgsHelper ArgsHelper { get; }

        public Type InstanceType { get; }

        protected ConductorAllActiveViewModel([NotNull] ArgsHelper argsArgsHelper, bool includeNonPublic = false)
        {
            ArgsHelper = argsArgsHelper;
            InstanceType = this.Initialize(includeNonPublic);
        }
    }
}