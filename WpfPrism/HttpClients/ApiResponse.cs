using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrism.HttpClients
{
    /// <summary>
    /// Api接受
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Msg { get; set; }
    }
}
