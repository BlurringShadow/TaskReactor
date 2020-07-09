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
    public sealed partial class GraphEditPageViewModel : ScreenViewModel
    {
        [NotNull] private UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(UserProfileViewModel))]
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _ = RefreshData();
                Set(ref _currentUser, value);
            }
        }

        [NotNull] readonly IUserTaskService _taskService;

        [NotNull] readonly ITaskDependencyService _taskDependencyService;

        [NotNull] readonly EditorControl _control = new EditorControl();

        TaskGraphArea _area;

        public EditorControl.EditorMode Mode
        {
            get => _control.Mode;
            set
            {
                _control.Mode = value;
                NotifyOfPropertyChange();
            }
        }

        public string ModePresentStr =>
            Mode switch
            {
                EditorControl.EditorMode.Move => "移动",
                EditorControl.EditorMode.Link => "创建依赖",
                _ => throw new ArgumentOutOfRangeException()
            };

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

        public void OnBindingGraphArea([NotNull] TaskGraphArea area) => _area = area;

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
            await Task.Run(
                () =>
                {
                    while (_area is null) ;
                }
            );

            var taskList = await _taskService.GetAllFromUserAsync(CurrentUser);
            var dependencyTaskList = new List<Task<List<TaskDependencyModel>>>(taskList.Count);

            foreach (var task in taskList)
            {
                dependencyTaskList.Add(_taskDependencyService.GetDependenciesAsync(task!));

                {
                    var vertex = Container.GetExportedValue<UserTaskVertex>();

                    vertex.ID = task.Identity;

                    // ReSharper disable once PossibleNullReferenceException
                    _area.AddVertex(vertex, new TaskVertexControl(vertex));
                }
            }

            // ReSharper disable PossibleNullReferenceException
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var dependenciesTask in dependencyTaskList)
            foreach (var edgeData in (await dependenciesTask).Select(
                dependency => new TaskDependencyEdge { Model = dependency }
            ))
            {
                edgeData.SetLinkVertexByModel(_area.LogicCore.Graph);

                var vertexList = _area.VertexList;
                _area.AddEdge(
                    edgeData, new EdgeControl(vertexList[edgeData.Source], vertexList[edgeData.Target], edgeData)
                );
            }

            _area.RelayoutGraph(true);

            // ReSharper restore PossibleNullReferenceException
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}