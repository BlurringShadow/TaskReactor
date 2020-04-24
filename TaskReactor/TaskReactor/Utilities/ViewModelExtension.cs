using System;
using JetBrains.Annotations;

namespace TaskReactor.Utilities
{
    public static class ViewModelExtension
    {
        [NotNull]
        public static Type Initialize([NotNull] this IViewModel viewModel, bool includeNonPublic = false)
        {
            var instanceType = viewModel.GetType();
            viewModel.ArgsHelper.Inject(instanceType, includeNonPublic);

            return instanceType;
        }
    }
}