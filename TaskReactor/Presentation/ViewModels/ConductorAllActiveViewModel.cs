using System;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Implementation for <see cref="Conductor{T}.Collection.AllActive"/> and <see cref="IViewModel"/>
    /// </summary>
    public abstract class ConductorAllActiveViewModel<T> : Conductor<T>.Collection.AllActive, IViewModel where T : class
    {
        public CompositionContainer Container { get; }

        public Type InstanceType { get; }

        protected ConductorAllActiveViewModel([NotNull] CompositionContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}