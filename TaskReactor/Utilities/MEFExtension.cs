using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities
{
    public static class MEFExtension
    {
        [NotNull]
        public static string GetMEFContractName([NotNull] this Type type) =>
            AttributedModelServices.GetContractName(type) ??
            throw new InvalidOperationException("Cannot get the contract name of the type");

        public static void UpdateExportedValue<T>([NotNull] this CompositionContainer container,
            T value,
            [NotNull] Func<ExportDefinition, bool> predicate,
            string contractName = null)
        {
            var batch = new CompositionBatch();

            if (contractName is null) batch.AddExportedValue(value);
            else batch.AddExportedValue(contractName, value);

            // enumerate the parts
            foreach (var partDefinition in from partDefinition in container.Catalog!.Parts!
                where partDefinition != null &&
                      partDefinition.ExportDefinitions != null &&
                      partDefinition.ExportDefinitions.Any(predicate)
                // ReSharper disable once PossibleNullReferenceException
                select partDefinition) batch.RemovePart(partDefinition.CreatePart());

            container.Compose(batch);
        }
    }
}