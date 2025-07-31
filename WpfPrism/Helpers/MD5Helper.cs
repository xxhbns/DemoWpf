using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrism.Helpers
{
    /// <summary>
    /// MD5 工具类
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMD5(string content)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(content)).Select(x => x.ToString("x2")));
        }
    }
}
