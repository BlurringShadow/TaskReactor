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
using GraphX.Common.Interfaces;
using GraphX.Controls.Models;
using JetBrains.Annotations;
using Presentation.Views.UserProfile.TaskDependencyGraph;
using QuickGraph;
using Utilities;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    using TaskGraph = BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>;
    using TaskGraphLogicCore = IGXLogicCore<
        UserTaskVertex,
        TaskDependencyEdge,
        BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>;

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed partial class GraphEditPageViewModel : ScreenViewModel
    {
        [NotNull] UserModel _currentUser;

        [NotNull, ShareVariable(nameof(CurrentUser), typeof(UserProfileViewModel))]
        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                Task.Run(
                    async () =>
                    {
                        while (!CanRefreshData) ;
                        await RefreshData();
                    }
                );
                Set(ref _currentUser, value);
            }
        }

        [NotNull] public IUserTaskService TaskService { get; set; }

        [NotNull] public ITaskDependencyService TaskDependencyService { get; set; }

        [NotNull] readonly EditorControl _control = new EditorControl();

        TaskGraphLogicCore _logicCore;

        [NotNull] TaskGraph _graph => _logicCore!.Graph!;

        public event Action RefreshGraph;

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

        public string ModePresentStr => _control.Mode switch
        {
            EditorControl.EditorMode.Move => "移动",
            EditorControl.EditorMode.Link => "创建依赖",
            _ => throw new ArgumentOutOfRangeException()
        };

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GraphEditPageViewModel([NotNull] IocContainer container) : base(container)
        {
        }

        public void OnBindingGraphLogicCore([NotNull] TaskGraphLogicCore core) => _logicCore = core;

        public async Task OnSelectedVertex(VertexSelectedEventArgs args)
        {
            if (await _control.OnSelectedVertex(
                args?.VertexControl!.GetDataVertex<UserTaskVertex>()!, _graph,
                TaskDependencyService
            )) RefreshGraph?.Invoke();
        }

        public async Task OnDoubleClickEdge(EdgeSelectedEventArgs args)
        {
            if (await _control.OnDoubleClickEdge(
                args?.EdgeControl!.GetDataEdge<TaskDependencyEdge>()!, _graph,
                TaskDependencyService
            )) RefreshGraph?.Invoke();
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            Task.Run(
                async () =>
                {
                    while (!CanRefreshData) ;
                    await RefreshData();
                }, cancellationToken
            );
            return base.OnActivateAsync(cancellationToken);
        }

        public bool CanRefreshData => !(_logicCore is null);

        public async Task RefreshData() => await Task.Run(RefreshDataImpl);

        void RefreshDataImpl()
        {
            IList<UserTaskModel> taskList;

            lock (TaskService) taskList = TaskService.GetAllFromUserAsync(CurrentUser).Result;

            var dependencyTaskList = new List<Task<List<TaskDependencyModel>>>(taskList.Count);

            // ReSharper disable once PossibleNullReferenceException
            lock (_logicCore)
            {
                _graph.Clear();

                foreach (var task in taskList)
                {
                    dependencyTaskList.Add(TaskDependencyService.GetDependenciesAsync(task!));
                    _graph.AddVertex(FromTaskToUserTaskVertex(task));
                }

                foreach (var edgeData in dependencyTaskList.SelectMany(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    dependenciesTask => dependenciesTask.Result.Select(
                        dependency => new TaskDependencyEdge { Model = dependency }
                    )
                ))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    edgeData.SetLinkVertexByModel(_graph);
                    _graph.AddEdge(edgeData);
                }

                RefreshGraph?.Invoke();
            }
        }

        UserTaskVertex FromTaskToUserTaskVertex(UserTaskModel task)
        {
            var vertex = Container.GetExportedValue<UserTaskVertex>();
            vertex.Task = task;
            return vertex;
        }
    }
}