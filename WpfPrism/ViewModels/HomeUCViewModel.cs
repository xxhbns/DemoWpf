using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Models.DTO;
using Prism.Regions;
using Prism.Services.Dialogs;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    public partial class HomeUCViewModel : ObservableValidator, INavigationAware
    {
        public HomeUCViewModel()
        {
            CreateStatPanelList();
            CreateWaitInfoList();
            CreateMemoInfoList();
        }

        [ObservableProperty]
        private string _loginInfo;

        /// <summary>
        /// 待办事项数据
        /// </summary>
        [ObservableProperty]
        private List<WaitInfoDTO> _waitInfoList;

        private void CreateWaitInfoList() 
        {
            WaitInfoList = [];
            WaitInfoList.Add(new WaitInfoDTO { Title = "会议一", Content = "会议内容" });
            WaitInfoList.Add(new WaitInfoDTO { Title = "会议二", Content = "会议内容" });
        }

        [ObservableProperty]
        private List<MemoInfoDTO> _memoInfoList;

        private void CreateMemoInfoList()
        {
            MemoInfoList = [];
            MemoInfoList.Add(new MemoInfoDTO { Title = "会议一", Content = "会议内容" });
            MemoInfoList.Add(new MemoInfoDTO { Title = "会议二", Content = "会议内容" });
        }

        /// <summary>
        /// 统计面板数据
        /// </summary>
        [ObservableProperty]
        private List<StatPanelInfo> _statPanelList;

        private void CreateStatPanelList()
        {
            StatPanelList = [];
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockFast", ItemName = "汇总", BackColor = "#FF0CA0FF", ViewName = "WaitUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockCheckOutline", ItemName = "已完成", BackColor = "#FF1ECA3A", ViewName = "WaitUC", Result = "9" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "CharLineVariant", ItemName = "完成比例", BackColor = "#FF02C6DC", ViewName = "", Result = "90%" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "PlaylistStar", ItemName = "备忘录", BackColor = "#FFFFA000", ViewName = "MemoUC", Result = "20" });
        }

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
    }
}
