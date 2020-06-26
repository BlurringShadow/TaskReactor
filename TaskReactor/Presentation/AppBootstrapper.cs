using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Data.Database;
using JetBrains.Annotations;
using Presentation.ViewModels;
using Utilities;

namespace Presentation
{
    public sealed class AppBootstrapper : BootstrapperBase
    {
        // IOC container
        [NotNull] private readonly CompositionContainer _container;

        public AppBootstrapper()
        {
            _container = new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(typeof(TaskReactorDbContext).Assembly),
                    new AssemblyCatalog(typeof(IViewModel).Assembly)
                ),
                true
            );
            Initialize();
        }

        protected override void Configure()
        {
            var batch = new CompositionBatch();

            // Instances for Caliburn.Micro 
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());

            // Add container itself
            batch.AddExportedValue(_container);

            // For view model share variable extension
            batch.AddExportedValue<IDictionary<(Type, string), ComposablePart>>(
                new ConcurrentDictionary<(Type, string), ComposablePart>()
            );

            _container.Compose(batch);
        }

        protected override object GetInstance([NotNull] Type serviceType, string contractName)
        {
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return _container.GetExports(serviceType, null, contractName).Single()!.Value;
            }
            catch (Exception e)
            {
                throw new NullReferenceException($"Can't export type:{serviceType.AssemblyQualifiedName}\n contract name:{contractName}", e);
            }
        }

        protected override IEnumerable<object> GetAllInstances([NotNull] Type serviceType) =>
            _container.GetExportedValues<object>(serviceType.GetMEFContractName()!);

        protected override void BuildUp([NotNull] object instance) => _container.SatisfyImportsOnce(instance);

        protected override void OnStartup(object sender, StartupEventArgs args)
        {
            try
            {
                _container.GetExportedValue<IWindowManager>()
                    .ShowWindowAsync(GetInstance(typeof(MainScreenViewModel), null));
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