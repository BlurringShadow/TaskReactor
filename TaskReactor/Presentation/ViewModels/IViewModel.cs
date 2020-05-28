using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Interface for view model
    /// </summary>
    public interface IViewModel : INotifyPropertyChangedEx
    {
        /// <summary>
        /// Container to store shared variable.
        /// </summary>
        [NotNull] CompositionContainer Container { get; }

        /// <summary>
        /// The type of current instance
        /// </summary>
        [NotNull] Type InstanceType { get; }
        
        [NotNull] IDictionary<(Type, string), ComposablePart> VariableParts { get; }
    }
}