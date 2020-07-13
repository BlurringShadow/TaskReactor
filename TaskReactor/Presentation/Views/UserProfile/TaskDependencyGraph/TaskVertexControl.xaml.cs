using System.Windows;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public partial class TaskVertexControl
    {
        static TaskVertexControl() => DefaultStyleKeyProperty?.OverrideMetadata(
            typeof(TaskVertexControl), new FrameworkPropertyMetadata(typeof(TaskVertexControl))
        );

        public TaskVertexControl(UserTaskVertex vertexData) : base(vertexData)
        {
            // ReSharper disable once PossibleNullReferenceException
            EventOptions.PositionChangeNotification = false;
        }

        public UserTaskVertex GetDataVertex() => (UserTaskVertex)Vertex;
    }
}