using System;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public abstract class ScreenViewModel : Screen, IViewModel
    {
        public CompositionContainer Container { get; }

        public Type InstanceType { get; }

        protected ScreenViewModel([NotNull] CompositionContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}