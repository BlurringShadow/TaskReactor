using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public abstract class ScreenViewModel : Screen, IViewModel
    {
        public CompositionContainer Container { get; }

        public Type InstanceType { get; }

        public IDictionary<(Type, string), ComposablePart> VariableParts { get; }

        protected ScreenViewModel(
            [NotNull] CompositionContainer container, 
            [NotNull] IDictionary<(Type, string), ComposablePart> variableParts
        )
        {
            Container = container;
            VariableParts = variableParts;
            InstanceType = this.Initialize();
        }
    }
}