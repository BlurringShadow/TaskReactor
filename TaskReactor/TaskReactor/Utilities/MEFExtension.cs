using System;
using JetBrains.Annotations;
using static System.ComponentModel.Composition.AttributedModelServices;

namespace TaskReactor.Utilities
{
    public static class MEFExtension
    {
        [NotNull]
        public static string GetMEFContractName([NotNull] this Type type) =>
            GetContractName(type) ?? throw new InvalidOperationException("Cannot get the contract name of the type");
    }
}