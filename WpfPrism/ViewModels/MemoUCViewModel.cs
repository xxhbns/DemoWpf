using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.DTO;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.HttpClients;

namespace WpfPrism.ViewModels
{
    public partial class MemoUCViewModel : ObservableValidator
    {
        /// <summary>
        /// http
        /// </summary>
        private readonly HttpRestClient httpRestClient;

        public MemoUCViewModel(HttpRestClient httpRestClient)
        {
            this.httpRestClient = httpRestClient;

            MemoInfoDTOList = [];

            GetMemoInfoDTOList();
        }

        #region 是否打开右侧添加备忘录栏
        [ObservableProperty]
        private bool _isShowAddMemo;

        [RelayCommand]
        private void ShowAddMemo()
        {
            IsShowAddMemo = true;
        }
        #endregion

        #region 获取备忘录数据
        [ObservableProperty]
        private List<MemoInfoDTO> _memoInfoDTOList;

        private void GetMemoInfoDTOList()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = "Memo/GetMemoList"
            };

            ApiResponse apiResponse = httpRestClient.Execute(apiRequest);
            if (apiResponse.IsSuccess)
            {
                MemoInfoDTOList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
            }
        }
        #endregion

        #region 查询备忘录数据
        [ObservableProperty]
        private string _searchTitle;

        [RelayCommand]
        private void QueryMemoInfos()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Memo/GetMemoList?title={SearchTitle}"
            };

            ApiResponse apiResponse = httpRestClient.Execute(apiRequest);
            if (apiResponse.IsSuccess)
            {
                MemoInfoDTOList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(apiResponse.Parameter.ToString());
            }
        }
        #endregion

    }
}
