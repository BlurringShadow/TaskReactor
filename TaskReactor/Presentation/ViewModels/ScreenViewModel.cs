using System;
using Caliburn.Micro;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    public abstract class ScreenViewModel : Screen, IViewModel
    {
        public IocContainer Container { get; }

        public Type InstanceType { get; }

        protected ScreenViewModel([NotNull] IocContainer container)
        {
            Container = container;
            InstanceType = this.Initialize();
        }
    }
}