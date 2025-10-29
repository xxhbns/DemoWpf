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
using WpfPrism.HttpClients.Interfaces;
using WpfPrism.HttpClients.Services;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    public partial class MemoUCViewModel : ObservableValidator, INavigationAware
    {
        /// <summary>
        /// http
        /// </summary>
        //private readonly HttpRestClient httpRestClient;

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// 备忘录API接口管理
        /// </summary>
        private readonly MemoServices _memoServices;

        /// <summary>
        /// 登录用户信息管理
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        public MemoUCViewModel(MemoServices memoServices, IEventAggregator eventAggregator, ICurrentUserService currentUserService)
        {
            _memoServices = memoServices;
            _eventAggregator = eventAggregator;
            _currentUserService = currentUserService;
            //this.httpRestClient = httpRestClient;

            ErrorHidden = Visibility.Hidden;
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
                if (MemoInfoDTOList == null || MemoInfoDTOList.Count == 0)
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

        #region 是否打开右侧添加备忘录栏
        [ObservableProperty]
        private bool _isShowAddMemo;

        [RelayCommand]
        private void ShowAddMemo()
        {
            IsShowAddMemo = true;
        }
        #endregion

        #region 查询备忘录数据
        [ObservableProperty]
        private List<MemoInfoDTO> _memoInfoDTOList;

        [ObservableProperty]
        private string _searchTitle;

        [RelayCommand]
        private async Task SearchMemoInfos()
        {
            await QueryMemoInfoDTOList();
        }

        /// <summary>
        /// 查询备忘录数据
        /// </summary>
        private async Task QueryMemoInfoDTOList()
        {
            ApiResponse apiResponse = await _memoServices.QueryMemoInfoDTOList(SearchTitle);
            if (apiResponse.IsSuccess)
            {
                MemoInfoDTOList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
            }
            else
            {
                MemoInfoDTOList = [];
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            }
            IsShowNoList(apiResponse.IsSuccess);
        }
        #endregion

        #region 添加备忘录
        /// <summary>
        /// 前台绑定的数据
        /// </summary>
        public MemoInfoDTO MemoInfoDTO { get; set; } = new MemoInfoDTO();

        [RelayCommand]
        private async Task AddMemoInfo()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title) || string.IsNullOrEmpty(MemoInfoDTO.Contents))
            {
                MessageBox.Show("标题和内容不能为空");
                return;
            }

            //调用Api实现添加待办事项
            ApiResponse apiResponse = await _memoServices.AddMemoInfo(MemoInfoDTO);
            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                //MemoInfoDTOList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
                await QueryMemoInfoDTOList();

                IsShowAddMemo = false;
            }
        }
        #endregion

        #region 删除备忘录
        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="memoInfoDTO"></param>
        [RelayCommand]
        private async Task DeleteMemoInfo(MemoInfoDTO memoInfoDTO) 
        {
            if (MessageBox.Show("确定删除此条备忘录？", "温馨提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK) 
            {
                //调用API
                ApiResponse apiResponse = await _memoServices.DeleteMemoInfo(memoInfoDTO);
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                if (apiResponse.IsSuccess)
                {
                    await QueryMemoInfoDTOList();
                }
            }
        }
        #endregion

        #region 区域导航
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _ = SafeExecuteHelper.SafeExecuteAsync(async () =>
            {
                //SearchTitle = "";
                await QueryMemoInfoDTOList();
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
