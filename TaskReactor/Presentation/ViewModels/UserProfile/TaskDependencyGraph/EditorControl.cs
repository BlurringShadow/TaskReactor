#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 下午 4:18

#endregion

using System.Threading.Tasks;
using ApplicationDomain.ModelService;
using GraphX.Controls;
using JetBrains.Annotations;
using Presentation.Views.UserProfile.TaskDependencyGraph;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    public partial class GraphEditPageViewModel
    {
        public class EditorControl
        {
            public enum EditorMode
            {
                Move,
                Link
            }

            public EditorMode Mode { get; set; }

            VertexControl _selectedVertexControl;

            public async Task OnSelectedVertex(
                VertexControl vertexControl,
                [NotNull] TaskGraphArea area,
                [NotNull] ITaskDependencyService service
            )
            {
                var dstData = vertexControl?.GetDataVertex<UserTaskVertex>();

                if (Mode != EditorMode.Link || dstData is null) return;

                var sourceData = _selectedVertexControl?.GetDataVertex<UserTaskVertex>();

                if (sourceData is null ||
                    sourceData.Task!.StartTime > dstData!.Task!.StartTime ||
                    area.LogicCore!.Graph!.ContainsEdge(dstData, sourceData))
                {
                    _selectedVertexControl = vertexControl;
                    return;
                }

                try
                {
                    var dependencyModel = service.AddDependencies(dstData.Task, sourceData.Task)[0];
                    await service.DbSync();

                    var data = new TaskDependencyEdge(dstData, sourceData) { Model = dependencyModel };
                    area.AddEdge(data, new EdgeControl(_selectedVertexControl, vertexControl, data));
                }
                catch
                {
                    // ignored
                }
            }

            public async Task OnDoubleClickEdge(
                EdgeControl edgeControl,
                [NotNull] TaskGraphArea area,
                [NotNull] ITaskDependencyService service
            )
            {
                var data = edgeControl?.GetDataEdge<TaskDependencyEdge>();
                if (data is null) return;

                try
                {
                    service.Remove(data.Model!);
                    await service.DbSync();
                    area.RemoveEdge(data);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}