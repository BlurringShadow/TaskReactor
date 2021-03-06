﻿#region File Info

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
using Caliburn.Micro;
using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using JetBrains.Annotations;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserTaskVertexViewModel : PropertyChangedBase, IGraphXVertex, IEquatable<UserTaskVertexViewModel>
    {
        [NotNull] readonly IUserTaskService _service;

        [ImportingConstructor]
        public UserTaskVertexViewModel([NotNull] IUserTaskService service) => _service = service;

        UserTaskModel _task;

        public UserTaskModel Task { get => _task; set => Set(ref _task, value); }

        public string TimeStampPresent => $"{Task?.StartTime}—{Task?.EndTime}";

        public double Angle { get; set; }

        public int GroupId { get; set; }

        public ProcessingOptionEnum SkipProcessing { get; set; }

        public long ID { get => Task?.Identity ?? 0; set => Task = _service.FindByKeysAsync(value).Result; }

        public bool Equals(IGraphXVertex other) => other is UserTaskVertexViewModel vertex && Equals(vertex);

        public bool Equals(UserTaskVertexViewModel other) => ID == other?.ID;

        public override bool Equals(object obj) => obj is UserTaskVertexViewModel vertex && Equals(vertex);

        public override int GetHashCode() => ID.GetHashCode();
    }
}