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

namespace WpfPrism.ViewModels
{
    public partial class ToDoUCViewModel : ObservableValidator, INavigationAware
    {
        /// <summary>
        /// http
        /// </summary>
        //private readonly HttpRestClient httpRestClient;

        /// <summary>
        /// 待办事项API接口管理
        /// </summary>
        private readonly WaitServices _waitServices;

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public ToDoUCViewModel(WaitServices waitServices, IEventAggregator eventAggregator)
        {
            _waitServices = waitServices;
            //this.httpRestClient = httpRestClient;
            _eventAggregator = eventAggregator;
            SearchIndex = 2;
        }

        #region 列表为空或出错时显示的界面控制
        [ObservableProperty]
        private Visibility _noListHidden;

        [ObservableProperty]
        private Visibility _errorHidden;

        /// <summary>
        /// 是否显示列表
        /// </summary>
        private void IsShowNoList(bool IsSuccess)
        {
            if (IsSuccess)
            {
                if (WaitInfoDTOList == null || WaitInfoDTOList.Count == 0)
                {
                    NoListHidden = Visibility.Visible;
                }
                else NoListHidden = Visibility.Hidden;
                ErrorHidden = Visibility.Hidden;
            }
            else
            {
                ErrorHidden = Visibility.Visible;
                NoListHidden = Visibility.Hidden;
            }
        }

        #endregion

        #region 查询待办事项数据
        /// <summary>
        /// 待办事项数据集合
        /// </summary>
        [ObservableProperty]
        private List<WaitInfoDTO> _waitInfoDTOList;

        /// <summary>
        /// 待办事项查询标题条件
        /// </summary>
        [ObservableProperty]
        private string _searchTitle;

        /// <summary>
        /// 待办事项查询筛选框条件
        /// </summary>
        [ObservableProperty]
        private int _searchIndex;

        /// <summary>
        /// 查询待办事项数据命令
        /// </summary>
        [RelayCommand]
        private async Task SearchWaitInfoList() 
        {
            await QueryWaitInfoDTOList();
        }

        /// <summary>
        /// 查询待办事项数据
        /// </summary>
        private async Task QueryWaitInfoDTOList()
        {
            //通过Api获取待办事项数据
            ApiResponse apiResponse = await _waitServices.QueryWaitInfoDTOList(SearchTitle, SearchIndex);
            if (apiResponse.IsSuccess) 
            {
                WaitInfoDTOList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
            }
            else
            {
                WaitInfoDTOList = [];
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            }
            IsShowNoList(apiResponse.IsSuccess);
        }
        #endregion

        #region 是否打开右侧添加待办事项栏
        [ObservableProperty]
        private bool _isShowAddWait;

        [RelayCommand]
        private void ShowAddWait()
        {
            IsShowAddWait = true;
        }
        #endregion

        #region 添加待办事项
        /// <summary>
        /// 前台绑定的数据
        /// </summary>
        public WaitInfoDTO WaitInfoDTO { get; set; } = new WaitInfoDTO();

        [RelayCommand]
        private async Task AddWaitInfo()
        {
            if (string.IsNullOrEmpty(WaitInfoDTO.Title) || string.IsNullOrEmpty(WaitInfoDTO.Contents))
            {
                return;
            }

            //调用Api实现添加待办事项
            ApiResponse apiResponse = await _waitServices.AddWaitInfo(WaitInfoDTO);
            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                //WaitInfoDTOList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
                await QueryWaitInfoDTOList();
                IsShowAddWait = false;
            }
        }
        #endregion

        #region 删除待办事项
        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="waitInfoDTO"></param>
        [RelayCommand]
        private async Task DeleteWaitInfo(WaitInfoDTO waitInfoDTO)
        {
            if (MessageBox.Show("确定删除此条待办事项？", "温馨提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //调用API
                ApiResponse apiResponse = await _waitServices.DeleteWaitInfo(waitInfoDTO);
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                if (apiResponse.IsSuccess)
                {
                    await QueryWaitInfoDTOList();
                }
            }
        }
        #endregion

        #region 区域导航
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _ = SafeExecuteHelper.SafeExecuteAsync(async () =>
            {
                if (navigationContext.Parameters.TryGetValue<int>("HomeTOToDo_SearchIndex", out var index))
                {
                    SearchIndex = index;
                }
                else
                    SearchIndex = 2;
                SearchTitle = "";

                await QueryWaitInfoDTOList();
            });
        }

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
