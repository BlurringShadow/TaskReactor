using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using JetBrains.Annotations;
using Presentation.ViewModels;
using Utilities;
using static System.Reflection.Assembly;

namespace Presentation
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        // IOC container
        [NotNull] private readonly CompositionContainer _container;

        public AppBootstrapper()
        {
            _container = new CompositionContainer(new AssemblyCatalog(GetExecutingAssembly()));
            Initialize();
        }

        protected override void Configure()
        {
            var batch = new CompositionBatch();

            // Instances for Caliburn.Micro 
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());

            _container.Compose(batch);
        }

        protected override object GetInstance([NotNull] Type serviceType, [NotNull] string contractName) =>
            _container.GetExportedValue<object>(
                (string.IsNullOrEmpty(contractName) ? serviceType.GetMEFContractName() : contractName)!
            );

        protected override IEnumerable<object> GetAllInstances([NotNull] Type serviceType) =>
            _container.GetExportedValues<object>(serviceType.GetMEFContractName()!);

        protected override void BuildUp([NotNull] object instance) => _container.SatisfyImportsOnce(instance);

        protected override void OnStartup(object sender, StartupEventArgs args)
        {
            try
            {
                _container.GetExportedValue<IWindowManager>()
                    .ShowWindowAsync(_container.GetExportedValue<MainScreenViewModel>());
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"From {sender}, {e.Message}",
                    "An error as occurred.",
                    MessageBoxButton.OK
                );
                Environment.Exit(0);
            }
        }

        protected override void OnUnhandledException(object sender, [NotNull] DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(
                $"From {sender}, {e.Exception?.Message}",
                "An error as occurred.",
                MessageBoxButton.OK
            );
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
            base.OnExit(sender, e);
        }
    }
}