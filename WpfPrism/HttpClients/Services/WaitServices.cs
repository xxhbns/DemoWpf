using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.HttpClients.Interfaces;

namespace WpfPrism.HttpClients.Services
{
    /// <summary>
    /// 待办事项API接口管理
    /// </summary>
    public class WaitServices
    {
        /// <summary>
        /// http
        /// </summary>
        private readonly HttpRestClient _httpRestClient;

        /// <summary>
        /// 登录用户信息管理
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        public WaitServices(HttpRestClient httpRestClient, ICurrentUserService currentUserService)
        {
            _httpRestClient = httpRestClient;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// 待办事项面板统计
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetStatWait()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Wait/GetStatWait?account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse; 
        }

        /// <summary>
        /// 获取待办事项数据
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetWaitInfoList()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Wait/GetWaitInfo?status=0&account={_currentUserService.Account}",
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 根据条件查询待办事项数据
        /// </summary>
        /// <param name="SearchTitle"></param>
        /// <param name="SearchIndex"></param>
        /// <returns></returns>
        public async Task<ApiResponse> QueryWaitInfoDTOList(string? SearchTitle, int SearchIndex)
        {
            //通过Api获取待办事项数据
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Wait/GetWaitInfo?title={SearchTitle}&account={_currentUserService.Account}"
            };
            if (SearchIndex != 2)
            {
                apiRequest.Route += $"&status={SearchIndex}";
            }
            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        ///添加待办事项 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> AddWaitInfo(WaitInfoDTO addModel)
        {
            addModel.Account = _currentUserService.Account;
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.POST,
                Parameters = addModel,
                Route = "Wait/AddWaitInfo"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 修改待办事项
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> UpdateWaitInfo(WaitInfoDTO NewWaitModel)
        {
            NewWaitModel.Account = _currentUserService.Account;
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.PUT,
                Parameters = NewWaitModel,
                Route = "Wait/UpdateWaitInfo"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="waitInfoDTO"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DeleteWaitInfo(WaitInfoDTO waitInfoDTO)
        {
            //调用API
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"Wait/DelWaitList?waitId={waitInfoDTO.WaitId}&account={_currentUserService.Account}"
            };

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }
    }
}
