using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    /// <summary>
    /// 接受API统计待办事项DTO
    /// </summary>
    public class StatWaitDTO
    {
        /// <summary>
        /// 待办事项总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int FinishCount { get; set; }

        /// <summary>
        /// 完成比例
        /// </summary>
        public string FinishPercent { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }
    }
}
