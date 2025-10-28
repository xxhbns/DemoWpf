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
using WpfPrism.HttpClients;
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

        public MemoUCViewModel(MemoServices memoServices, IEventAggregator eventAggregator)
        {
            _memoServices = memoServices;
            _eventAggregator = eventAggregator;
            //this.httpRestClient = httpRestClient;

            QueryMemoInfoDTOList();
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
        private void SearchMemoInfos()
        {
            QueryMemoInfoDTOList();
        }

        /// <summary>
        /// 查询备忘录数据
        /// </summary>
        private void QueryMemoInfoDTOList()
        {
            ApiResponse apiResponse = _memoServices.QueryMemoInfoDTOList(SearchTitle);
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
        private void AddMemoInfo()
        {
            if (string.IsNullOrEmpty(MemoInfoDTO.Title) || string.IsNullOrEmpty(MemoInfoDTO.Contents))
            {
                return;
            }

            //调用Api实现添加待办事项
            ApiResponse apiResponse = _memoServices.AddMemoInfo(MemoInfoDTO);
            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                //MemoInfoDTOList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
                QueryMemoInfoDTOList();

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
        private void DeleteMemoInfo(MemoInfoDTO memoInfoDTO) 
        {
            if (MessageBox.Show("确定删除此条备忘录？", "温馨提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK) 
            {
                //调用API
                ApiResponse apiResponse = _memoServices.DeleteMemoInfo(memoInfoDTO);
                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                if (apiResponse.IsSuccess)
                {
                    QueryMemoInfoDTOList();
                }
            }
        }
        #endregion

        #region 区域导航
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //SearchTitle = "";
            //QueryMemoInfoDTOList();
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
