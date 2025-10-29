using Models.DTO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.HttpClients.Interfaces;

namespace WpfPrism.HttpClients.Services
{
    /// <summary>
    /// 备忘录API接口管理
    /// </summary>
    public class MemoServices
    {
        /// <summary>
        /// http
        /// </summary>
        private readonly HttpRestClient _httpRestClient;

        /// <summary>
        /// 登录用户信息管理
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <param name="httpRestClient"></param>
        public MemoServices(HttpRestClient httpRestClient, ICurrentUserService currentUserService)
        {
            _httpRestClient = httpRestClient;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 待办事项面板统计
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetStatMemo() 
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Memo/StatMemoInfo?account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 获取备忘录数据
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetMemoInfoList()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Memo/GetMemoList?account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 根据标题查询备忘录数据
        /// </summary>
        /// <param name="SearchTitle"></param>
        /// <returns></returns>
        public async Task<ApiResponse> QueryMemoInfoDTOList(string? SearchTitle)
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Memo/GetMemoList?title={SearchTitle}&account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="addModel"></param>
        /// <returns></returns>
        public async Task<ApiResponse> AddMemoInfo(MemoInfoDTO addModel)
        {
            addModel.Account = _currentUserService.Account;
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.POST,
                Parameters = addModel,
                Route = "Memo/AddMemoInfo"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 修改备忘录
        /// </summary>
        /// <param name="NewMemoModel"></param>
        /// <returns></returns>
        public async Task<ApiResponse> UpdateMemoInfo(MemoInfoDTO NewMemoModel)
        {
            NewMemoModel.Account = _currentUserService.Account;
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.PUT,
                Parameters = NewMemoModel,
                Route = "Memo/UpdateMemoInfo"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="memoInfoDTO"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DeleteMemoInfo(MemoInfoDTO memoInfoDTO)
        {
            //调用API
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"Memo/DelMemoList?memoId={memoInfoDTO.MemoId}&account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }
    }
}
