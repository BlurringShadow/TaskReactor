using System.ComponentModel.Composition.Hosting;
using Data.Database.Entity;
using JetBrains.Annotations;
using Presentation;

namespace UnitTest.ApplicationDomain.Repository
{
    public abstract class RepositoryTestBase
    {
        [NotNull] public static CompositionContainer Container { get; } = new CompositionContainer(
            new AggregateCatalog(
                new AssemblyCatalog(typeof(IDatabaseModel).Assembly!),
                new AssemblyCatalog(typeof(App).Assembly!)
            ),
            true
        );
    }
}