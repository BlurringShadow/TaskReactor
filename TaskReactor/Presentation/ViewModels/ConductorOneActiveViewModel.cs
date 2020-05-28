using System;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public abstract class ConductorOneActiveViewModel<T> : Conductor<T>.Collection.OneActive, IViewModel where T : class
    {
        public CompositionContainer Container { get; }

        public Type InstanceType { get; }

        protected ConductorOneActiveViewModel([NotNull] CompositionContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}