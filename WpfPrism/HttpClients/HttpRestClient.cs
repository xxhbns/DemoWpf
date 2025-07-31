using System;
using System.Collections.Generic;
using System.Linq;
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

        public HttpRestClient()
        {
            _restClient = new RestClient();
        }

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
    }
}
