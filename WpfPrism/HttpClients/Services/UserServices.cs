using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.Helpers;
using static DryIoc.ServiceInfo;

namespace WpfPrism.HttpClients.Services
{
    /// <summary>
    /// 用户API接口管理
    /// </summary>
    public class UserServices
    {
        /// <summary>
        /// http
        /// </summary>
        private readonly HttpRestClient _httpRestClient;

        public UserServices(HttpRestClient httpRestClient)
        {
            _httpRestClient = httpRestClient;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Account">账号</param>
        /// <param name="MyPwd">密码</param>
        /// <returns></returns>
        public async Task<ApiResponse> UserLoginAsync(string Account, string MyPwd) 
        {
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.GET
            };

            MyPwd = MD5Helper.GetMD5(MyPwd);
            apiRequest.Route = $"Users/Login?account={Account}&password={MyPwd}";

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="usersReq">用户注册信息</param>
        /// <returns></returns>
        public async Task<ApiResponse> UserRegAsync(UsersReq usersReq)
        {
            //调用Api
            ApiRequest apiRequest = new()
            {
                Method = RestSharp.Method.POST,
                Route = "Users/Reg"
            };

            //对密码进行处理
            usersReq.Password = MD5Helper.GetMD5(usersReq.Password);

            apiRequest.Parameters = usersReq;

            ApiResponse apiResponse = await _httpRestClient.ExecuteAsync(apiRequest);
            return apiResponse;
        }
    }
}
