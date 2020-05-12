using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Utilities
{
    public interface IViewModel : INotifyPropertyChangedEx
    {
        [NotNull] ArgsHelper ArgsHelper { get; }
        [NotNull] Type InstanceType { get; }
    }
}