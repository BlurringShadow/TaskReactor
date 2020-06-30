using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Implementation for <see cref="Conductor{T}.Collection.AllActive"/> and <see cref="IViewModel"/>
    /// </summary>
    public abstract class ConductorAllActiveViewModel<T> : Conductor<T>.Collection.AllActive, IViewModel where T : class
    {
        public IocContainer Container { get; }

        public Type InstanceType { get; }

        protected ConductorAllActiveViewModel([NotNull] IocContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}