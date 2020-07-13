#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 上午 11:03

#endregion

using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Logic.Models;
using QuickGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public class TaskGraphArea :
        GraphArea<UserTaskVertex, TaskDependencyEdge, BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>
    {
        public TaskGraphArea()
        {
            LogicCore = new GXLogicCore<UserTaskVertex, TaskDependencyEdge,
                BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama
            };
        }
    }
}