#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 下午 4:18

#endregion

using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using JetBrains.Annotations;
using Presentation.Views.UserProfile.TaskDependencyGraph;
using QuickGraph;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    public partial class GraphEditPageViewModel
    {
        internal class EditorControl
        {
            public enum EditorMode
            {
                Move,
                Link
            }

            public EditorMode Mode { get; set; }

            UserTaskVertexViewModel _selectedVertexViewModel;

            public async Task<bool> OnSelectedVertex(
                [NotNull] UserTaskVertexViewModel vertexViewModel,
                [NotNull] BidirectionalGraph<UserTaskVertexViewModel, TaskDependencyEdge> graph,
                [NotNull] ITaskDependencyService service
            )
            {
                if (Mode != EditorMode.Link) return false;

                return await Task.Run(
                    () =>
                    {
                        lock (graph)
                        {
                            if (_selectedVertexViewModel is null ||
                                _selectedVertexViewModel.Task!.StartTime > vertexViewModel!.Task!.StartTime ||
                                graph.ContainsEdge(vertexViewModel, _selectedVertexViewModel))
                            {
                                _selectedVertexViewModel = vertexViewModel;
                                return false;
                            }

                            TaskDependencyModel dependencyModel;
                            lock (service)
                            {
                                dependencyModel = service.AddDependencies(vertexViewModel.Task!, _selectedVertexViewModel.Task)[0];
                                Task.WaitAll(service.DbSync());
                            }

                            var data = new TaskDependencyEdge(vertexViewModel, _selectedVertexViewModel) { Model = dependencyModel };
                            graph.AddEdge(data);
                            _selectedVertexViewModel = null;
                        }

                        return true;
                    }
                );
            }

            public async Task<bool> OnDoubleClickEdge(
                [NotNull] TaskDependencyEdge edge,
                [NotNull] BidirectionalGraph<UserTaskVertexViewModel, TaskDependencyEdge> graph,
                [NotNull] ITaskDependencyService service
            ) => await Task.Run(
                () =>
                {
                    lock (graph)
                        if (graph.ContainsEdge(edge))
                        {
                            graph.RemoveEdge(edge);
                            lock (service)
                            {
                                service.Remove(edge.Model!);
                                Task.WaitAll(service.DbSync());
                            }

                            return true;
                        }

                    return false;
                }
            );
        }
    }
}