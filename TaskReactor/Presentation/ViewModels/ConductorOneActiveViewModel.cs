using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Implementation for <see cref="Conductor{T}.Collection.OneActive"/> and <see cref="IViewModel"/>
    /// </summary>
    public abstract class ConductorOneActiveViewModel<T> : Conductor<T>.Collection.OneActive, IViewModel where T : class
    {
        public IocContainer Container { get; }

        public Type InstanceType { get; }

        protected ConductorOneActiveViewModel([NotNull] IocContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}