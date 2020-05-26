using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Presentation.ViewModels
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

        public static void Update<TTargetViewModel>(
            [NotNull] this IViewModel viewModel,
            object value,
            [CallerMemberName] string key = null
        ) => Update(viewModel, typeof(TTargetViewModel), value, key);

        public static void Update([NotNull] this IViewModel viewModel,
            [NotNull] Type targetType,
            object value,
            [CallerMemberName] string key = null
        ) => viewModel.ArgsHelper.Update(viewModel.InstanceType, targetType, value, key);
    }
}