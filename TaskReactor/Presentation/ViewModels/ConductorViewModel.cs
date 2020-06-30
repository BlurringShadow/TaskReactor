#region File Info

// Created by 张泽益,
// Year: 2020
// Month: 06
// Day: 09
// Time: 下午 3:03

#endregion

using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    /// <summary>
    /// Implementation for <see cref="Conductor{T}"/> and <see cref="IViewModel"/>
    /// </summary>
    public abstract class ConductorViewModel<T> : Conductor<T>, IViewModel where T : class
    {
        public IocContainer Container { get; }

        public Type InstanceType { get; }

        protected ConductorViewModel([NotNull] IocContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}