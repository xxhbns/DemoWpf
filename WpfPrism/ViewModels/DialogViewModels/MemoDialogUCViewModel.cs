using CommunityToolkit.Mvvm.ComponentModel;
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
    /// 备忘录添加、编辑弹窗数据模型
    /// </summary>
    public class MemoDialogUCViewModel : IDialogHostAwre
    {
        private const string DialogHostName = "RootDialog";

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public MemoDialogUCViewModel(IEventAggregator eventAggregator)
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
        public MemoInfoDTO MemoInfoDTO { get; set; } = new MemoInfoDTO();

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
                    ShowTitle = "添加备忘录";
                }
                else 
                {
                    ShowTitle = "编辑备忘录";
                }
            }
            if (parameters.TryGetValue<MemoInfoDTO>("OldMemoInfo", out var oldMemoModle))
            {
                MemoInfoDTO = oldMemoModle;
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("备忘录的标题必须填写！");
                return;
            }
            if (string.IsNullOrEmpty(MemoInfoDTO.Contents))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("备忘录的内容必须填写！");
                return;
            }

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK,
                    new DialogParameters()
                    {
                        { "NewMemoInfo", MemoInfoDTO }
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
