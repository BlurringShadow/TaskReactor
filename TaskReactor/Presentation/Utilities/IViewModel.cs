using System;
using JetBrains.Annotations;

namespace Presentation.Utilities
{
    public interface IViewModel
    {
        [NotNull] ArgsHelper ArgsHelper { get; }
        [NotNull] Type InstanceType { get; }
    }
}