﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
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

        public IDictionary<(Type, string), ComposablePart> VariableParts { get; }

        protected ConductorAllActiveViewModel(
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