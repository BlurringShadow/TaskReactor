﻿using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Presentation.ViewModels
{
    internal static class ViewModelExtension
    {
        /// <summary>
        /// Initialize the view model
        /// </summary>
        /// <param name="viewModel"> the view model </param>
        /// <returns> Type of the view model </returns>
        [NotNull]
        internal static Type Initialize([NotNull] this IViewModel viewModel) => viewModel.GetType();

        /// <summary>
        /// Extension for view model to share variable.
        /// If the specified variable exists, it will be overwrote by new value.  
        /// </summary>
        /// <param name="viewModel"> View model to share </param>
        /// <param name="value"> The variable to shared </param>
        /// <param name="contractName"> Name of the variable </param>
        internal static void Share<TVariable>(
            [NotNull] this IViewModel viewModel,
            [NotNull] TVariable value,
            [CallerMemberName] string contractName = null
        )
        {
            var container = viewModel.Container;
            container.UpdateExportedValue(value, definition => definition?.ContractName == contractName, contractName);
        }

        /// <summary>
        /// Extension for view model to import share variable
        /// </summary>
        /// <param name="viewModel"> Referenced view model </param>
        /// <param name="contractName"> Name of the variable </param>
        /// <returns> Shared variable </returns>
        [NotNull,
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        internal static TVariable GetShared<TViewModel, TVariable>(
            [NotNull] this TViewModel viewModel,
            [CallerMemberName] string contractName = null
        ) where TViewModel : IViewModel => viewModel.Container.GetExportedValue<TVariable>(contractName);


        /// <summary>
        /// Extension for view model to share variable.
        /// Build unique contract name relative to <paramref name="viewModel.InstanceType"/> and variableName
        /// </summary>
        /// <param name="viewModel"> View model to share </param>
        /// <param name="value"> The variable to shared </param>
        [NotNull]
        internal static ShareVariableBuilder<TVariable> ForVariable<TViewModel, TVariable>(
            [NotNull] this TViewModel viewModel,
            [NotNull] TVariable value
        ) where TViewModel : IViewModel => new ShareVariableBuilder<TVariable>(viewModel, value);


        /// <summary>
        /// Extension for view model to share variable.
        /// Build unique contract name relative to <paramref name="viewModel.InstanceType"/> and <paramref name="variableName"/>
        /// </summary>
        /// <param name="viewModel"> View model to share </param>
        /// <param name="value"> The variable to shared </param>
        /// <param name="variableName"> Name of the variable </param>
        internal static void ShareWithName<TViewModel, TVariable>(
            [NotNull] this TViewModel viewModel,
            [NotNull] TVariable value,
            [CallerMemberName] string variableName = null
        ) where TViewModel : IViewModel => viewModel.ForVariable(value).WithName(variableName).Share();
    }
}