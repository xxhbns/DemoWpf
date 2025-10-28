using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace WpfPrism.HttpClients
{
    public class HttpRestClient
    {
        private readonly string baseUrl = "http://localhost:14761/api/";

        private readonly RestClient _restClient;

        //private readonly HttpClient _httpClient;

        public HttpRestClient()//HttpClient httpClient)
        {
            _restClient = new RestClient();
            //_httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri(baseUrl);
            //_httpClient.DefaultRequestHeaders.Accept.Clear();
            //_httpClient.DefaultRequestHeaders.Accept.Add(
            //    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region 请求
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="req">请求数据</param>
        /// <returns>接收的数据</returns>
        public ApiResponse Execute(ApiRequest req)
        {
            RestRequest restRequest = new RestRequest(req.Method);//请求方法
            restRequest.AddHeader("Content-Type", req.ContentType);//内容类型

            if (req.Parameters != null) //参数
            {
                //SerializeObject json序列化 对象->json字符串
                restRequest.AddParameter("param", JsonConvert.SerializeObject(req.Parameters), ParameterType.RequestBody);

            }

            _restClient.BaseUrl = new Uri(baseUrl + req.Route);
            var res = _restClient.Execute(restRequest);//发送请求
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //DeserializeObject json反序列化 json字符串->对象
                return JsonConvert.DeserializeObject<ApiResponse>(res.Content);
            }
            else
            {
                return new ApiResponse { IsSuccess = false, Msg = "服务器繁忙，请稍等" };
            }
        } 
        #endregion

        #region 异步请求方法
        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="req">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<ApiResponse> ExecuteAsync(ApiRequest req, CancellationToken cancellationToken = default)
        {
            try
            {
                RestRequest restRequest = new RestRequest(req.Method);
                restRequest.AddHeader("Content-Type", req.ContentType);

                if (req.Parameters != null)
                {
                    restRequest.AddParameter("param", JsonConvert.SerializeObject(req.Parameters), ParameterType.RequestBody);
                }

                _restClient.BaseUrl = new Uri(baseUrl + req.Route);

                // 传递取消令牌
                var response = await _restClient.ExecuteAsync(restRequest, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Msg = $"请求失败: {response.StatusCode}"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Msg = "请求已取消"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Msg = $"请求异常: {ex.Message}"
                };
            }
        } 
        #endregion

        #region 使用HttpClient的异步方法
        /// <summary>
        /// 使用HttpClient的异步方法
        /// </summary>
        //public async Task<ApiResponse> ExecuteAsync(ApiRequest req)
        //{
        //    try
        //    {
        //        HttpRequestMessage request = new HttpRequestMessage(
        //            new HttpMethod(req.Method),
        //            req.Route);

        //        if (req.Parameters != null)
        //        {
        //            var jsonContent = JsonConvert.SerializeObject(req.Parameters);
        //            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //        }

        //        var response = await _httpClient.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<ApiResponse>(content);
        //        }
        //        else
        //        {
        //            return new ApiResponse
        //            {
        //                IsSuccess = false,
        //                Msg = $"HTTP错误: {response.StatusCode}"
        //            };
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return new ApiResponse
        //        {
        //            IsSuccess = false,
        //            Msg = $"网络请求失败: {ex.Message}"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse
        //        {
        //            IsSuccess = false,
        //            Msg = $"请求异常: {ex.Message}"
        //        };
        //    }
        //} 
        #endregion

    }
}
