using System;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Use MEF to import the share variable
    /// </summary>
    internal class ShareVariableAttribute : ViewModelComponentAttribute
    {
        /// <summary>
        /// Create the attribute with variable name and type
        /// </summary>
        public ShareVariableAttribute(string variableName, Type fromType) :
            base(ShareVariableNameBuilder.CreateFrom(fromType).WithName(variableName).ContractName)
        {
        }
    }
}