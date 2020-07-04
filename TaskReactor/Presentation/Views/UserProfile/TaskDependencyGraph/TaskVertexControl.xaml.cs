using System.Windows;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public partial class TaskVertexControl
    {
        static TaskVertexControl() => DefaultStyleKeyProperty?.OverrideMetadata(
            typeof(TaskVertexControl), new FrameworkPropertyMetadata(typeof(TaskVertexControl))
        );

        public TaskVertexControl(UserTaskVertex vertexData, bool tracePositionChange = false, bool bindToDataObject = true) :
            base(vertexData, tracePositionChange, bindToDataObject)
        {
        }
    }
}