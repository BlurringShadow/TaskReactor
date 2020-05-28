using System;
using System.ComponentModel.Composition;

namespace Presentation.ViewModels
{
    internal class ShareVariableAttribute : ImportAttribute
    {
        public ShareVariableAttribute(Type contractType) : base(contractType) => AllowRecomposition = true;

        public ShareVariableAttribute(string contractName) : base(contractName) => AllowRecomposition = true;

        public ShareVariableAttribute(string contractName, Type contractType) : base(contractName, contractType) =>
            AllowRecomposition = true;
    }
}