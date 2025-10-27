using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;
using Newtonsoft.Json;
using Prism.Regions;
using Prism.Services.Dialogs;
using WpfPrism.HttpClients;

namespace WpfPrism.ViewModels
{
    public partial class ToDoUCViewModel : ObservableValidator, INavigationAware
    {
        /// <summary>
        /// http
        /// </summary>
        private readonly HttpRestClient httpRestClient;

        public ToDoUCViewModel(HttpRestClient httpRestClient)
        {
            this.httpRestClient = httpRestClient;
            SearchIndex = 2;
        }

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
        private void SearchWaitInfoList() 
        {
            QueryWaitInfoDTOList();
        }

        /// <summary>
        /// 查询待办事项数据
        /// </summary>
        private void QueryWaitInfoDTOList()
        {
            //通过Api获取待办事项数据
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Wait/GetWaitInfo?title={SearchTitle}"
            };
            if (SearchIndex != 2)
            {
                apiRequest.Route += $"&status={SearchIndex}";
            }
            ApiResponse apiResponse = httpRestClient.Execute(apiRequest);
            if (apiResponse.IsSuccess) 
            {
                WaitInfoDTOList = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(apiResponse.Parameter.ToString());
            }
            else
            {
                WaitInfoDTOList = [];
            }
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

        #region 区域导航
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<int>("HomeTOToDo_SearchIndex", out var index)) 
            {
                SearchIndex = index;
            }
            else 
                SearchIndex = 2;
            QueryWaitInfoDTOList();
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
