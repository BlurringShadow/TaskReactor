using System;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    internal class ShareVariableNameBuilder
    {
        [NotNull]
        public static ShareVariableNameBuilder CreateFrom(Type fromViewModelType) =>
            new ShareVariableNameBuilder(fromViewModelType);

        public Type FromViewModelType { get; }
        public string VariableName { get; private set; }
        public string ContractName => $"{VariableName ?? ""}:{FromViewModelType?.Name ?? ""}";

        private ShareVariableNameBuilder(Type fromViewModelType) => FromViewModelType = fromViewModelType;

        [NotNull]
        public ShareVariableNameBuilder WithName(string variableName)
        {
            VariableName = variableName;
            return this;
        }
    }
}