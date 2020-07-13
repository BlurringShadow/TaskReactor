#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 06
// Time: 下午 3:37

#endregion

using System;
using System.Linq;
using ApplicationDomain.DataModel;
using GraphX.Common.Models;
using JetBrains.Annotations;
using QuickGraph;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    public class TaskDependencyEdge : EdgeBase<UserTaskVertex>
    {
        public TaskDependencyModel Model { get; set; }

        public TaskDependencyEdge(UserTaskVertex source = null, UserTaskVertex target = null) : base(source, target)
        {
        }

        public void SetLinkVertexByModel([NotNull] BidirectionalGraph<UserTaskVertex, TaskDependencyEdge> graph)
        {
            if (Model is null) throw new NullReferenceException();
            Source = (from vertex in graph.Vertices where vertex.ID == Model.Dependency.Identity select vertex).Single();
            Target = (from vertex in graph.Vertices where vertex.ID == Model.Target.Identity select vertex).Single();
        }
    }
}