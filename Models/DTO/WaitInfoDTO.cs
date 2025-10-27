using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    /// <summary>
    /// 待办事项DTO
    /// </summary>
    public class WaitInfoDTO
    {
        /// <summary>
        /// 待办事项Id
        /// </summary>
        public int WaitId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BackColor
        { 
            get 
            {
                return Status == 0 ? "#1E90FF" : "#3CB371";
            } 
        }
    }
}
