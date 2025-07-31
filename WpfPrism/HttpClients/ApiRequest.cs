using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WpfPrism.HttpClients
{
    /// <summary>
    /// Api请求
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 请求方式(Get/Post/Put/Delate)
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// 发送的类型
        /// </summary>
        public string ContentType { get; set; } = "application/json";
    }
}
