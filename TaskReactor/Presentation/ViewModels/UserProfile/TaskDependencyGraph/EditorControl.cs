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

            UserTaskVertex _selectedVertex;

            public async Task<bool> OnSelectedVertex(
                [NotNull] UserTaskVertex vertex,
                [NotNull] BidirectionalGraph<UserTaskVertex, TaskDependencyEdge> graph,
                [NotNull] ITaskDependencyService service
            )
            {
                if (Mode != EditorMode.Link) return false;

                return await Task.Run(
                    () =>
                    {
                        lock (graph)
                        {
                            if (_selectedVertex is null ||
                                _selectedVertex.Task!.StartTime > vertex!.Task!.StartTime ||
                                graph.ContainsEdge(vertex, _selectedVertex))
                            {
                                _selectedVertex = vertex;
                                return false;
                            }

                            TaskDependencyModel dependencyModel;
                            lock (service)
                            {
                                dependencyModel = service.AddDependencies(vertex.Task!, _selectedVertex.Task)[0];
                                Task.WaitAll(service.DbSync());
                            }

                            var data = new TaskDependencyEdge(vertex, _selectedVertex) { Model = dependencyModel };
                            graph.AddEdge(data);
                            _selectedVertex = null;
                        }

                        return true;
                    }
                );
            }

            public async Task<bool> OnDoubleClickEdge(
                [NotNull] TaskDependencyEdge edge,
                [NotNull] BidirectionalGraph<UserTaskVertex, TaskDependencyEdge> graph,
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