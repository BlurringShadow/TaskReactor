#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 07
// Day: 05
// Time: 下午 4:18

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationDomain.DataModel;
using ApplicationDomain.ModelService;
using GraphX.Controls;
using GraphX.Controls.Models;
using JetBrains.Annotations;
using Presentation.Views.UserProfile.TaskDependencyGraph;
using Utilities;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    [Export]
    public class GraphEditPageViewModel : ScreenViewModel
    {
        public class EditorControl
        {
            public enum EditorMode
            {
                Move,
                Link
            }

            public EditorMode Mode { get; set; }

            VertexControl _selectedVertexControl;

            public async Task OnSelectedVertex(
                VertexControl vertexControl,
                [NotNull] TaskGraphArea area,
                [NotNull] ITaskDependencyService service
            )
            {
                var dstData = vertexControl?.GetDataVertex<UserTaskVertex>();

                if (Mode != EditorMode.Link || dstData is null) return;

                var sourceData = _selectedVertexControl?.GetDataVertex<UserTaskVertex>();

                if (sourceData is null ||
                    sourceData.Task!.StartTime > dstData!.Task!.StartTime ||
                    area.LogicCore!.Graph!.ContainsEdge(dstData, sourceData))
                {
                    _selectedVertexControl = vertexControl;
                    return;
                }

                TaskDependencyModel dependencyModel;
                try
                {
                    dependencyModel = service.AddDependencies(dstData.Task, sourceData.Task)[0];
                    await service.DbSync();
                }
                catch (Exception) { return; }

                var data = new TaskDependencyEdge(dstData, sourceData) { Model = dependencyModel };
                area.AddEdge(data, new EdgeControl(_selectedVertexControl, vertexControl, data));
            }

            public async Task OnDoubleClickEdge(
                EdgeControl edgeControl,
                [NotNull] TaskGraphArea area,
                [NotNull] ITaskDependencyService service
            )
            {
                var data = edgeControl?.GetDataEdge<TaskDependencyEdge>();
                if (data is null) return;

                try
                {
                    service.Remove(data.Model!);
                    await service.DbSync();
                }
                catch (Exception) { return; }

                area.RemoveEdge(data);
            }
        }

        [NotNull, ShareVariable(nameof(CurrentUserModel), typeof(UserProfileViewModel))] 
        public UserModel CurrentUserModel { get; set; }

        [NotNull] readonly IUserTaskService _taskService;

        [NotNull] readonly ITaskDependencyService _taskDependencyService;

        [NotNull] readonly EditorControl _control = new EditorControl();

        public EditorControl.EditorMode Mode
        {
            get => _control.Mode;
            set
            {
                _control.Mode = value;
                NotifyOfPropertyChange();
            }
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GraphEditPageViewModel(
            [NotNull] IocContainer container, 
            [NotNull] ITaskDependencyService service, 
            [NotNull] IUserTaskService taskService
        ) : base(container)
        {
            _taskDependencyService = service;
            _taskService = taskService;
        }

        public async Task OnSelectedVertex(object sender, VertexSelectedEventArgs args) =>
            await _control.OnSelectedVertex(args?.VertexControl, ((TaskGraphArea)sender)!, _taskDependencyService);

        public async Task OnDoubleClickEdge(object sender, EdgeSelectedEventArgs args) =>
            await _control.OnDoubleClickEdge(args?.EdgeControl, ((TaskGraphArea)sender)!, _taskDependencyService);

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _ = RefreshData();
            return base.OnActivateAsync(cancellationToken);
        }

        public async Task RefreshData()
        {
            var taskList = await _taskService.GetAllFromUserAsync(CurrentUserModel);
            foreach (var task in taskList)
            {
                var dependenciesTask = _taskDependencyService.GetDependenciesAsync(task!);

                // TODO Add vertex

                // TODO Add edges
                // ReSharper disable once UnusedVariable
                foreach (var dependency in await dependenciesTask)
                {
                }
            }
        }
    }
}