#region File Info

// Created by Blurring Shadow,
// Year: 2020
// Month: 06
// Day: 08
// Time: 下午 5:37

#endregion

using System;
using System.ComponentModel.Composition;

namespace Presentation.ViewModels
{
    public class ViewModelComponentAttribute : ImportAttribute
    {
        public ViewModelComponentAttribute() => AllowRecomposition = true;

        public ViewModelComponentAttribute(string contractName) : base(contractName) => AllowRecomposition = true;

        public ViewModelComponentAttribute(Type contractType) : base(contractType) => AllowRecomposition = true;

        public ViewModelComponentAttribute(string contractName, Type contractType) : base(contractName, contractType) =>
            AllowRecomposition = true;
    }
}