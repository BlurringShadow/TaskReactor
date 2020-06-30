using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Interface for view model
    /// </summary>
    internal interface IViewModel : INotifyPropertyChangedEx
    {
        /// <summary>
        /// Container to store shared variable.
        /// </summary>
        [NotNull] IocContainer Container { get; }

        /// <summary>
        /// The type of current instance
        /// </summary>
        [NotNull] Type InstanceType { get; }
    }
}