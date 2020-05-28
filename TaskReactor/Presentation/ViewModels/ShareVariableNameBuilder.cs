using System;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    internal class ShareVariableNameBuilder
    {
        public Type FromViewModelType { get; }
        public string VariableName { get; private set; }
        public string ContractName => $"{FromViewModelType?.Name ?? ""}:{VariableName ?? ""}";

        public ShareVariableNameBuilder(Type fromViewModelType) => FromViewModelType = fromViewModelType;

        [NotNull]
        public ShareVariableNameBuilder WithName(string variableName)
        {
            VariableName = variableName;
            return this;
        }
    }
}