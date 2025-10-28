using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public WaitServices(HttpRestClient httpRestClient)
        {
            _httpRestClient = httpRestClient;
        }

        /// <summary>
        /// 待办事项面板统计
        /// </summary>
        /// <returns></returns>
        public ApiResponse GetStatWait()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = "Wait/GetStatWait"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse; 
        }

        /// <summary>
        /// 获取待办事项数据
        /// </summary>
        /// <returns></returns>
        public ApiResponse GetWaitInfoList()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Wait/GetWaitInfo?status=0",
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 根据条件查询待办事项数据
        /// </summary>
        /// <param name="SearchTitle"></param>
        /// <param name="SearchIndex"></param>
        /// <returns></returns>
        public ApiResponse QueryWaitInfoDTOList(string? SearchTitle, int SearchIndex)
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
            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        ///添加待办事项 
        /// </summary>
        /// <returns></returns>
        public ApiResponse AddWaitInfo(WaitInfoDTO addModel)
        {
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.POST,
                Parameters = addModel,
                Route = "Wait/AddWaitInfo"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 修改待办事项
        /// </summary>
        /// <returns></returns>
        public ApiResponse UpdateWaitInfo(WaitInfoDTO NewWaitModel)
        {
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.PUT,
                Parameters = NewWaitModel,
                Route = "Wait/UpdateWaitInfo"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="waitInfoDTO"></param>
        /// <returns></returns>
        public ApiResponse DeleteWaitInfo(WaitInfoDTO waitInfoDTO)
        {
            //调用API
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"Wait/DelWaitList?waitId={waitInfoDTO.WaitId}"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }
    }
}
