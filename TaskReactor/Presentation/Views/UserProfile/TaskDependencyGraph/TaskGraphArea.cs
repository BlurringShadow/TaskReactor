#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 上午 11:03

#endregion

using GraphX;
using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using QuickGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public class TaskGraphArea :
        GraphArea<UserTaskVertex, TaskDependencyEdge, BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>
    {
        class TaskGraphControlFactory : GraphControlFactory
        {
            public TaskGraphControlFactory(GraphAreaBase graphArea) : base(graphArea)
            {
            }

            public override VertexControl CreateVertexControl(object vertexData) =>
                new TaskVertexControl((UserTaskVertex)vertexData);
        }

        public TaskGraphArea()
        {
            LogicCore = new GXLogicCore<UserTaskVertex, TaskDependencyEdge,
                BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama
            };

            ControlFactory = new TaskGraphControlFactory(this);
        }
    }
}