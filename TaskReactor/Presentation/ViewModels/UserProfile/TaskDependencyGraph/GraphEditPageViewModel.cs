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
using System.Windows;
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

        public bool ModeValue
        {
            get => _control.Mode != 0;
            set
            {
                _control.Mode = value ? (EditorControl.EditorMode)1 : 0;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(ModePresentStr));
            }
        }

        public string ModePresentStr =>
            _control.Mode switch
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleNullReferenceException"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public async Task RefreshData()
        {
            IList<UserTaskModel> taskList = null;

            await Task.Run(
                () =>
                {
                    while (_area is null) ;
                    lock (_taskService) taskList = _taskService.GetAllFromUserAsync(CurrentUser).Result;
                }
            );

            var dependencyTaskList = new List<Task<List<TaskDependencyModel>>>(taskList.Count);

            var areaDispatcher = _area.Dispatcher;

            try
            {
                lock (areaDispatcher)
                {
                    areaDispatcher.Invoke(() => _area.ClearLayout());

                    foreach (var task in taskList)
                    {
                        dependencyTaskList.Add(_taskDependencyService.GetDependenciesAsync(task!));

                        {
                            var vertex = Container.GetExportedValue<UserTaskVertex>();

                            vertex.Task = task;

                            areaDispatcher.Invoke(() => _area.AddVertex(vertex, new TaskVertexControl(vertex)));
                        }
                    }


                    foreach (var edgeData in dependencyTaskList.SelectMany(
                        dependenciesTask => dependenciesTask.Result.Select(
                            dependency => new TaskDependencyEdge { Model = dependency }
                        )
                    ))
                    {
                        edgeData.SetLinkVertexByModel(_area.LogicCore.Graph);

                        var vertexList = _area.VertexList;

                        areaDispatcher.Invoke(
                            () => _area.AddEdge(
                                edgeData, new EdgeControl(
                                    vertexList[edgeData.Source], vertexList[edgeData.Target], edgeData
                                )
                            )
                        );
                    }

                    areaDispatcher.Invoke(() => _area.RelayoutGraph(true));
                }
            }
            catch (Exception e) { MessageBox.Show($"{e}"); }
        }
    }
}