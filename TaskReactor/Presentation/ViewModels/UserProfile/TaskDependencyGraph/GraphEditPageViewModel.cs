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
using Caliburn.Micro;
using GraphX.Common.Interfaces;
using GraphX.Controls.Models;
using JetBrains.Annotations;
using Presentation.Views.UserProfile.TaskDependencyGraph;
using QuickGraph;
using Utilities;
using Action = System.Action;

namespace Presentation.ViewModels.UserProfile.TaskDependencyGraph
{
    using TaskGraph = BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>;
    using TaskGraphLogicCore = IGXLogicCore<
        UserTaskVertex,
        TaskDependencyEdge,
        BidirectionalGraph<UserTaskVertex, TaskDependencyEdge>>;

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed partial class GraphEditPageViewModel : ScreenViewModel, IAsyncDisposable, IDisposable
    {
        [NotNull] UserModel _currentUser;

        [NotNull] public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _ = RefreshData();
                Set(ref _currentUser, value);
            }
        }

        [NotNull, Import] public IUserTaskService TaskService { get; set; }

        [NotNull, Import] public ITaskDependencyService TaskDependencyService { get; set; }

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

        CancellationTokenSource _refreshDataTokenSource;

        // ReSharper disable once NotNullMemberIsNotInitialized
        [ImportingConstructor]
        public GraphEditPageViewModel([NotNull] IocContainer container) : base(container)
        {
        }

        public void OnBindingGraphLogicCore([NotNull] TaskGraphArea area)
        {
            _logicCore = area.LogicCore;
            _ = RefreshData();
            NotifyOfPropertyChange(nameof(CanRefreshData));
        }

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

        protected override Task OnActivateAsync(CancellationToken token)
        {
            _refreshDataTokenSource ??= new CancellationTokenSource();
            return base.OnActivateAsync(_refreshDataTokenSource.Token);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (!_refreshDataTokenSource!.IsCancellationRequested)
            {
                _refreshDataTokenSource.Cancel();
                _refreshDataTokenSource.Dispose();
                // ReSharper disable once AssignNullToNotNullAttribute
                _refreshDataTokenSource = close ? null : new CancellationTokenSource();
            }

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public bool CanRefreshData => !(_logicCore is null);

        public async Task RefreshData() => await RefreshData(CancellationToken.None);

        public async Task RefreshData(CancellationToken externalToken)
        {
            if (!CanRefreshData) return;

            var token = CancellationTokenSource.CreateLinkedTokenSource(
                externalToken, _refreshDataTokenSource!.Token
            ).Token;

            await Task.Run(() => RefreshDataImpl(token), token);
        }

        void RefreshDataImpl(CancellationToken token)
        {
            // ReSharper disable once PossibleNullReferenceException
            lock (_logicCore)
            {
                _graph.Clear();

                var taskList = TaskService.GetAllFromUserAsync(CurrentUser, token).Result;

                var dependencyTaskList = new List<Task<List<TaskDependencyModel>>>(taskList.Count);

                foreach (var task in taskList)
                {
                    if (token.IsCancellationRequested) return;
                    dependencyTaskList.Add(TaskDependencyService.GetDependenciesAsync(task!, token));
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
                    if (token.IsCancellationRequested) return;

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

        public ValueTask DisposeAsync()
        {
            if (IsActive)
            {
                var task = this.DeactivateAsync(true);
                if (!(task is null))
                    return new ValueTask(task);
            }

            return default;
        }

        public void Dispose() => DisposeAsync().AsTask().Wait();
    }
}