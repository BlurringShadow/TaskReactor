using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using TaskReactor.ViewModels;

namespace TaskReactor
{
    sealed class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Instance(_container);

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            _container
                .PerRequest<MainViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<MainViewModel>();

        protected override object GetInstance(Type service, string key) => _container?.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service) => _container?.GetAllInstances(service);

        protected override void BuildUp(object instance) => _container?.BuildUp(instance);

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if(e == null) return;

            e.Handled = true;
            MessageBox.Show(e.Exception?.Message, "An error as occurred", MessageBoxButton.OK);
        }
    }
}