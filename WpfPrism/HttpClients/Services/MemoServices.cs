using Models.DTO;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <param name="httpRestClient"></param>
        public MemoServices(HttpRestClient httpRestClient)
        {
            _httpRestClient = httpRestClient;
        }

        /// <summary>
        /// 待办事项面板统计
        /// </summary>
        /// <returns></returns>
        public ApiResponse GetStatMemo() 
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = "Memo/StatMemoInfo"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 获取备忘录数据
        /// </summary>
        /// <returns></returns>
        public ApiResponse GetMemoInfoList()
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = "Memo/GetMemoList"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 根据标题查询备忘录数据
        /// </summary>
        /// <param name="SearchTitle"></param>
        /// <returns></returns>
        public ApiResponse QueryMemoInfoDTOList(string? SearchTitle)
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET,
                Route = $"Memo/GetMemoList?title={SearchTitle}"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="addModel"></param>
        /// <returns></returns>
        public ApiResponse AddMemoInfo(MemoInfoDTO addModel)
        {
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.POST,
                Parameters = addModel,
                Route = "Memo/AddMemoInfo"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 修改备忘录
        /// </summary>
        /// <param name="NewMemoModel"></param>
        /// <returns></returns>
        public ApiResponse UpdateMemoInfo(MemoInfoDTO NewMemoModel)
        {
            //调用Api实现添加待办事项
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.PUT,
                Parameters = NewMemoModel,
                Route = "Memo/UpdateMemoInfo"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="memoInfoDTO"></param>
        /// <returns></returns>
        public ApiResponse DeleteMemoInfo(MemoInfoDTO memoInfoDTO)
        {
            //调用API
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.DELETE,
                Route = $"Memo/DelMemoList?memoId={memoInfoDTO.MemoId}"
            };

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            return apiResponse;
        }
    }
}
