using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace TaskReactor.Utilities
{
    public abstract class ConductorOneActiveViewModel<T> : Conductor<T>.Collection.OneActive, IViewModel where T : class
    {
        public ArgsHelper ArgsHelper { get; }

        public Type InstanceType { get; }

        protected ConductorOneActiveViewModel([NotNull] ArgsHelper argsArgsHelper, bool includeNonPublic = false)
        {
            ArgsHelper = argsArgsHelper;
            InstanceType = this.Initialize(includeNonPublic);
        }
    }
}