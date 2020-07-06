using System.Windows;
using JetBrains.Annotations;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public partial class TaskVertexControl
    {
        static TaskVertexControl() => DefaultStyleKeyProperty?.OverrideMetadata(
            typeof(TaskVertexControl), new FrameworkPropertyMetadata(typeof(TaskVertexControl))
        );

        public TaskVertexControl([NotNull] UserTaskVertex vertexData, bool tracePositionChange, bool bindToDataObject = true) :
            base(vertexData, tracePositionChange, bindToDataObject)
        {
        }

        public UserTaskVertex GetDataVertex() => (UserTaskVertex)Vertex;
    }
}