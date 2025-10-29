using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    /// <summary>
    /// 备忘录DTO
    /// </summary>
    public class MemoInfoDTO
    {
        /// <summary>
        /// 备忘录Id
        /// </summary>
        public int MemoId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }
    }
}
