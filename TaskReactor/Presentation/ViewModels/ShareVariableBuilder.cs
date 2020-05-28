using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    internal class ShareVariableBuilder<TVariable>
    {
        [NotNull] public IViewModel ViewModel { get; }

        [NotNull] public TVariable Variable { get; }

        [NotNull] public ShareVariableNameBuilder NameBuilder { get; }

        public ShareVariableBuilder([NotNull] IViewModel viewModel, [NotNull] TVariable variable)
        {
            ViewModel = viewModel;
            Variable = variable;
            NameBuilder = new ShareVariableNameBuilder(viewModel.InstanceType);
        }

        /// <summary>
        /// Set the variable name
        /// </summary>
        /// <param name="variableName"> name of the variable </param>
        /// <returns></returns>
        [NotNull]
        public ShareVariableBuilder<TVariable> WithName([CallerMemberName] string variableName = null)
        {
            NameBuilder.WithName(variableName);
            return this;
        }

        /// <summary>
        /// Share the variable <see cref="ViewModelExtension.Share{TVariable}"/>
        /// </summary>
        public void Share() => ViewModel.Share(Variable, NameBuilder.ContractName);
    }
}