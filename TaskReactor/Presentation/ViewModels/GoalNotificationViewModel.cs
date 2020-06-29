using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Notifications.Wpf.Core;
using Caliburn.Micro;
using ApplicationDomain.DataModel;
using System.Threading;

namespace Presentation.ViewModels
{
    public sealed class GoalNotificationViewModel : PropertyChangedBase,INotificationViewModel
    {
            public string Title { get; set; }
            public string Message { get; set; }
            
            [NotNull] public INotificationManager Manager { get; }
            [NotNull] GoalModel _goalModel;

            public GoalModel GoalModel { get => _goalModel;}

            public GoalNotificationViewModel(INotificationManager manager)
            {
                Manager = manager;
            }

            public TimeSpan Duration
            {
                get
                {
                    return GoalModel.DurationOfOneTime;
                }
                set { }
            
            }

            public async Task ShowAsync()
                {
                    var content = new NotificationContent
                    {
                        Title = GoalModel.Title,
                        Message = GoalModel.Description,
                        Type = NotificationType.Information
                    };
                    //弹窗提示
                    await Manager.ShowAsync(content);
                }

            public async Task ShowAsync(CancellationToken token)
            {
                if (!token.IsCancellationRequested)
                {
                    var content = new NotificationContent
                    {
                        Title = GoalModel.Title,
                        Message = GoalModel.Description,
                        Type = NotificationType.Information
                    };
                    //弹窗提示
                    await Manager.ShowAsync(content);
            }
            }

            public event System.Action OnClick=ToGoalEditView;
            public event System.Action OnClose=Handle;
            
            public static void ToGoalEditView() {
                //跳到goalEditView，用户可在那个页面查看详情或者对计划作出修改
            }
            
            public static void Handle()
            {
                
            }

    }
}
