using MaterialDesignThemes.Wpf;
using Models.DTO;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.Helpers;
using WpfPrism.Models;
using WpfPrism.Services;

namespace WpfPrism.ViewModels.DialogViewModels
{
    /// <summary>
    /// 待办事项添加、编辑弹窗数据模型
    /// </summary>
    public class WaitDialogUCViewModel : IDialogHostAwre
    {
        private const string DialogHostName = "RootDialog";

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public WaitDialogUCViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string ShowTitle { get; set; }

        #region 对话框接口
        /// <summary>
        /// 确定命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 前台绑定的数据
        /// </summary>
        public WaitInfoDTO WaitInfoDTO { get; set; } = new WaitInfoDTO();

        /// <summary>
        /// 打开弹窗时的操作
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpening(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<DialogEnum>("DialogEnumType", out var dialogEnum))
            {
                if (dialogEnum == DialogEnum.Add)
                {
                    ShowTitle = "添加待办事项";
                }
                else
                {
                    ShowTitle = "编辑待办事项";
                }
            }
            if (parameters.TryGetValue<WaitInfoDTO>("OldWaitInfo", out var oldWaitModle))
            {
                WaitInfoDTO = oldWaitModle;
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(WaitInfoDTO.Title))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("待办事项的标题必须填写！");
                return;
            }
            if (string.IsNullOrEmpty(WaitInfoDTO.Contents))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("待办事项的内容必须填写！");
                return;
            }

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK,
                    new DialogParameters()
                    {
                        { "NewWaitInfo", WaitInfoDTO }
                    })
                );
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
            }
        }
        #endregion

    }
}
