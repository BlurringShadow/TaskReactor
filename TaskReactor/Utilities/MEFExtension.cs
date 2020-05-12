using System;
using System.ComponentModel.Composition;
using JetBrains.Annotations;

namespace Utilities
{
    public static class MEFExtension
    {
        [NotNull]
        public static string GetMEFContractName([NotNull] this Type type) =>
            AttributedModelServices.GetContractName(type) ?? throw new InvalidOperationException("Cannot get the contract name of the type");
    }
}