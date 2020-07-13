#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 04
// Time: 下午 10:17

#endregion

using System;
using System.ComponentModel.Composition;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using JetBrains.Annotations;

namespace Presentation.Views.UserProfile.TaskDependencyGraph
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserTaskVertex : IGraphXVertex, IEquatable<UserTaskVertex>
    {
        [NotNull] readonly IUserTaskService _service;

        [ImportingConstructor]
        public UserTaskVertex([NotNull] IUserTaskService service) => _service = service;

        public UserTaskModel Task { get; set; }

        public double Angle { get; set; }

        public int GroupId { get; set; }

        public ProcessingOptionEnum SkipProcessing { get; set; }

        public long ID { get => Task?.Identity ?? 0; set => Task = _service.FindByKeysAsync(value).Result; }

        public bool Equals(IGraphXVertex other) => other is UserTaskVertex vertex && Equals(vertex);

        public bool Equals(UserTaskVertex other) => ID == other?.ID;

        public override bool Equals(object obj) => obj is UserTaskVertex vertex && Equals(vertex);

        public override int GetHashCode() => ID.GetHashCode();
    }
}