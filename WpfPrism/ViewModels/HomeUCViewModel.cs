using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfPrism.Helpers;
using WpfPrism.HttpClients;
using WpfPrism.HttpClients.Services;
using WpfPrism.Models;
using WpfPrism.Services;

namespace WpfPrism.ViewModels
{
    public partial class HomeUCViewModel : ObservableValidator, INavigationAware
    {
        /// <summary>
        /// 待办事项API接口管理
        /// </summary>
        private readonly WaitServices _waitServices;

        /// <summary>
        /// 备忘录API接口管理
        /// </summary>
        private readonly MemoServices _memoServices;

        /// <summary>
        /// http
        /// </summary>
        //private readonly HttpRestClient httpRestClient;

        /// <summary>
        /// 自定义对话框服务
        /// </summary>
        private readonly DialogHostService dialogHostService;

        /// <summary>
        /// 区域管理
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public HomeUCViewModel(WaitServices waitServices, MemoServices memoServices, DialogHostService dialogHostService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _waitServices = waitServices;
            _memoServices = memoServices;
            //this.httpRestClient = httpRestClient;
            this.dialogHostService = dialogHostService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            GetPanelList();
            GetWaitInfoList();
            GetMemoInfoList();
            GetStatMemo();
        }

        #region 初始化时统计面板数据
        /// <summary>
        /// 统计面板数据
        /// </summary>
        [ObservableProperty]
        private List<StatPanelInfo> _statPanelList;

        /// <summary>
        /// 初始化时统计面板数据
        /// </summary>
        private void GetPanelList()
        {
            StatPanelList = [];
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockFast", ItemName = "汇总", BackColor = "#FF0CA0FF", ViewName = "ToDoUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockCheckOutline", ItemName = "已完成", BackColor = "#FF1ECA3A", ViewName = "ToDoUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "CharLineVariant", ItemName = "完成比例", BackColor = "#FF02C6DC", ViewName = "", Result = "99%" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "PlaylistStar", ItemName = "备忘录", BackColor = "#FFFFA000", ViewName = "MemoUC", Result = "99" });

            GetStatWait();
        }
        #endregion

        #region 控制面板导航
        [RelayCommand]
        private void Navigate(StatPanelInfo statPanelInfo)
        {
            if (!string.IsNullOrEmpty(statPanelInfo.ViewName))
            {
                if (statPanelInfo.ItemName == "已完成")
                {
                    _regionManager.Regions["MainViewRegion"].RequestNavigate(statPanelInfo.ViewName,
                        new NavigationParameters()
                        {
                            { "HomeTOToDo_SearchIndex", 1 }
                        }
                    );
                }
                else
                    _regionManager.Regions["MainViewRegion"].RequestNavigate(statPanelInfo.ViewName);
            }
        }
        #endregion

        #region 待办事项统计
        [ObservableProperty]
        private StatWaitDTO _statWait;

        /// <summary>
        /// 待办事项面板统计
        /// </summary>
        private void GetStatWait()
        {
            ApiResponse apiResponse = _waitServices.GetStatWait();
            if (apiResponse.IsSuccess)
            {
                StatWait = JsonConvert.DeserializeObject<StatWaitDTO>(apiResponse.Parameter.ToString());
                if (StatWait != null)
                {
                    RefreshWaitStat();
                }
                else
                {
                    _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                }
            }
        }

        #endregion

        #region 获取待办事项数据
        /// <summary>
        /// 待办事项数据
        /// </summary>
        [ObservableProperty]
        private List<WaitInfoDTO> _waitInfoList;

        /// <summary>
        /// 获取待办事项数据
        /// </summary>
        private void GetWaitInfoList()
        {
            WaitInfoList = [];

            ApiResponse apiResponse = _waitServices.GetWaitInfoList();
            if (apiResponse.IsSuccess)
            {
                WaitInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
                RefreshWaitStat();
            }
            else
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            }
        }

        /// <summary>
        /// 刷新面板数据
        /// </summary>
        private void RefreshWaitStat()
        {
            StatPanelList[0].Result = StatWait.TotalCount.ToString();
            StatPanelList[1].Result = StatWait.FinishCount.ToString();
            StatPanelList[2].Result = StatWait.FinishPercent;
        }
        #endregion

        #region 打开添加待办事项弹窗
        /// <summary>
        /// 打开添加待办事项对话框
        /// </summary>
        [RelayCommand]
        private async Task ShoWAddWaitDialog()
        {
            var result = await dialogHostService.ShowDialog("WaitDialogUC",
                new DialogParameters()
                {
                    { "DialogEnumType", DialogEnum.Add }
                }
            );

            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.TryGetValue<WaitInfoDTO>("NewWaitInfo", out var addModel))
                {
                    //调用Api实现添加待办事项
                    ApiResponse apiResponse = _waitServices.AddWaitInfo(addModel);
                    _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                    if (apiResponse.IsSuccess)
                    {
                        WaitInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
                        GetStatWait();
                    }
                }
            }
        }
        #endregion

        #region 修改待办事项
        [RelayCommand]
        private void UpdateWaitInfo(WaitInfoDTO waitInfoDTO)
        {
            ApiResponse apiResponse = _waitServices.UpdateWaitInfo(waitInfoDTO);

            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                GetStatWait();
            }
        }
        #endregion

        #region 点击打开编辑待办事项弹窗
        [RelayCommand]
        private async Task ShowEditWaitUC(WaitInfoDTO waitInfoDTO)
        {
            var result = await dialogHostService.ShowDialog("WaitDialogUC",
                new DialogParameters()
                {
                    { "OldWaitInfo", waitInfoDTO },
                    { "DialogEnumType", DialogEnum.Edit }
                }
            );

            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.TryGetValue<WaitInfoDTO>("NewWaitInfo", out var NewWaitModel))
                {
                    //调用Api实现添加待办事项
                    ApiResponse apiResponse = _waitServices.UpdateWaitInfo(NewWaitModel);

                    _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                    if (apiResponse.IsSuccess)
                    {
                        WaitInfoList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
                        GetStatWait();
                    }
                }
            }
        }
        #endregion

        #region 统计备忘录数据
        /// <summary>
        /// 统计备忘录数据
        /// </summary>
        private void GetStatMemo()
        {
            ApiResponse apiResponse = _memoServices.GetStatMemo();

            if (apiResponse.IsSuccess)
            {
                StatPanelList[3].Result = JsonConvert.DeserializeObject<string>(apiResponse.Parameter.ToString());
            }
            else
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            }
        }
        #endregion

        #region 获取备忘录数据
        [ObservableProperty]
        private List<MemoInfoDTO> _memoInfoList;

        /// <summary>
        /// 获取备忘录数据
        /// </summary>
        private void GetMemoInfoList()
        {
            MemoInfoList = [];

            ApiResponse apiResponse = _memoServices.GetMemoInfoList();
            if (apiResponse.IsSuccess)
            {
                MemoInfoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
            }
            else
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            }
        }
        #endregion

        #region 打开添加备忘录弹窗
        /// <summary>
        /// 打开添加备忘录弹窗
        /// </summary>
        [RelayCommand]
        private async Task ShoWAddMemoDialog()
        {
            var result = await dialogHostService.ShowDialog("MemoDialogUC",
                new DialogParameters()
                {
                    { "DialogEnumType", DialogEnum.Add }
                }
            );

            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.TryGetValue<MemoInfoDTO>("NewMemoInfo", out var addModel))
                {
                    //调用Api实现添加待办事项
                    ApiResponse apiResponse = _memoServices.AddMemoInfo(addModel);
                    _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                    if (apiResponse.IsSuccess)
                    {
                        MemoInfoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
                        GetStatMemo();
                    }
                }
            }
        }
        #endregion

        #region 点击打开编辑备忘录弹窗
        /// <summary>
        /// 点击打开编辑备忘录弹窗
        /// </summary>
        /// <param name="memoInfoDTO"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task ShowEditMemoUC(MemoInfoDTO memoInfoDTO)
        {
            var result = await dialogHostService.ShowDialog("MemoDialogUC",
                new DialogParameters()
                {
                    { "OldMemoInfo", memoInfoDTO },
                    { "DialogEnumType", DialogEnum.Edit }
                }
            );

            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.TryGetValue<MemoInfoDTO>("NewMemoInfo", out var NewMemoModel))
                {
                    //调用Api实现添加待办事项
                    ApiResponse apiResponse = _memoServices.UpdateMemoInfo(NewMemoModel);
                    _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                    if (apiResponse.IsSuccess)
                    {
                        MemoInfoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
                    }
                }
            }
        }
        #endregion

        #region 接收导航参数
        [ObservableProperty]
        private string _loginInfo;

        /// <summary>
        /// 接受导航
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<string>("LoginName", out var loginName))
            {
                DateTime dateTime = DateTime.Now;
                string[] weeks = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                LoginInfo = $"您好！{loginName}。今天是 {dateTime.ToString("yyyy-MM-dd")} {weeks[(int)dateTime.DayOfWeek]}";
            }
        }

        /// <summary>
        /// 是否导航
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

    }
}
