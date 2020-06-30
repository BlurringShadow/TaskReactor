#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 30
// Time: 上午 11:47

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Utilities
{
    public sealed class IocContainer : CompositionContainer
    {
        const BindingFlags _privateFieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        readonly ComposablePartExportProvider _composablePartExportProvider;

        // Exposed the composable part
        [NotNull, ItemNotNull] public IEnumerable<ComposablePart> ComposableParts =>
            ((IEnumerable<ComposablePart>)typeof(ComposablePartExportProvider)
                .GetField("_parts", _privateFieldBindingFlags)!.GetValue(_composablePartExportProvider))!;

        public IocContainer() : this((ComposablePartCatalog)null)
        {
        }

        public IocContainer(params ExportProvider[] providers) : this(null, providers)
        {
        }

        public IocContainer(CompositionOptions compositionOptions, params ExportProvider[] providers) :
            this(null, compositionOptions, providers)
        {
        }

        public IocContainer(ComposablePartCatalog catalog, params ExportProvider[] providers) :
            this(catalog, false, providers)
        {
        }

        public IocContainer(ComposablePartCatalog catalog, bool isThreadSafe, params ExportProvider[] providers)
            : this(catalog, isThreadSafe ? CompositionOptions.IsThreadSafe : CompositionOptions.Default, providers)
        {
        }

        public IocContainer(
            ComposablePartCatalog catalog,
            CompositionOptions compositionOptions,
            params ExportProvider[] providers
        ) : base(catalog, compositionOptions, providers) =>
            _composablePartExportProvider = (ComposablePartExportProvider)typeof(CompositionContainer)
                .GetField("_partExportProvider", _privateFieldBindingFlags)!.GetValue(this);

        public void UpdateExportedValue<T>(
            T value,
            [NotNull] Func<ExportDefinition, bool> predicate,
            string contractName = null
        )
        {
            var batch = new CompositionBatch();

            if (contractName is null) batch.AddExportedValue(value);
            else batch.AddExportedValue(contractName, value);

            // enumerate the parts
            foreach (var part in ComposableParts.Where(part => part.ExportDefinitions?.Any(predicate) == true))
                // ReSharper disable once AssignNullToNotNullAttribute
                batch.RemovePart(part);

            Compose(batch);
        }
    }
}