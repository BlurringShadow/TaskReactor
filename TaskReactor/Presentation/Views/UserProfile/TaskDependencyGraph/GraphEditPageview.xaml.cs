using System;
using Presentation.ViewModels.UserProfile.TaskDependencyGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public sealed partial class GraphEditPageView : IDisposable
    {
        public GraphEditPageView()
        {
            DataContextChanged += (sender, e) =>
            {
                if (!(e.NewValue is GraphEditPageViewModel viewModel)) return;

                // ReSharper disable once PossibleNullReferenceException
                viewModel.RefreshGraph += () => Dispatcher.Invoke(OnRefreshGraph);
            };

            InitializeComponent();

            // ReSharper disable once PossibleNullReferenceException
            ZoomControl.ZoomToFill();
        }

        void OnRefreshGraph()
        {
            GraphArea!.GenerateGraph();
            GraphArea.ShowAllEdgesLabels(false);
            ZoomControl!.ZoomToFill();
        }

        public void Dispose()
        {
            if (DataContext is GraphEditPageViewModel viewModel) viewModel!.RefreshGraph -= OnRefreshGraph;
            GraphArea?.Dispose();
        }
    }
}