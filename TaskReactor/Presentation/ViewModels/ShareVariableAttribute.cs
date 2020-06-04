using System;
using System.ComponentModel.Composition;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Use MEF to import the share variable
    /// </summary>
    internal class ShareVariableAttribute : ImportAttribute
    {
        /// <summary>
        /// Create the attribute with variable name and type
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="fromType"></param>
        public ShareVariableAttribute(string variableName, Type fromType) :
            base(ShareVariableNameBuilder.CreateFrom(fromType).WithName(variableName).ContractName) =>
            AllowRecomposition = true;
    }
}