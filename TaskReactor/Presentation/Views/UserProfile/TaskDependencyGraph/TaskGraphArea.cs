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
using Presentation.ViewModels.UserProfile.TaskDependencyGraph;
using QuickGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public class TaskGraphArea :
        GraphArea<UserTaskVertexViewModel, TaskDependencyEdge, BidirectionalGraph<UserTaskVertexViewModel, TaskDependencyEdge>>
    {
        public TaskGraphArea()
        {
            LogicCore = new GXLogicCore<UserTaskVertexViewModel, TaskDependencyEdge,
                BidirectionalGraph<UserTaskVertexViewModel, TaskDependencyEdge>>
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama
            };
        }
    }
}