using System;
using JetBrains.Annotations;

namespace TaskReactor.Utilities
{
    public interface IViewModel
    {
        [NotNull] ArgsHelper ArgsHelper { get; }
        [NotNull] Type InstanceType { get; }
    }
}