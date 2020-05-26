using System;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    public interface IViewModel : INotifyPropertyChangedEx
    {
        [NotNull] ArgsHelper ArgsHelper { get; }
        [NotNull] Type InstanceType { get; }
    }
}