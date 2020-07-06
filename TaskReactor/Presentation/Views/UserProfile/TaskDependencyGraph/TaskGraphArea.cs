#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 上午 11:03

#endregion

using GraphX.Common.Enums;
using GraphX.Common.Models;
using GraphX.Controls;
using GraphX.Logic.Models;
using QuickGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public class TaskGraphArea :
        GraphArea<UserTaskVertex, EdgeBase<UserTaskVertex>, 
            BidirectionalGraph<UserTaskVertex, EdgeBase<UserTaskVertex>>>
    {
        public TaskGraphArea()
        {
            LogicCore = new GXLogicCore<UserTaskVertex, EdgeBase<UserTaskVertex>,
                BidirectionalGraph<UserTaskVertex, EdgeBase<UserTaskVertex>>>
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama
            };
        }
    }
}