using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrism.Models
{
    /// <summary>
    /// 左侧菜单信息
    /// </summary>
    public class LeftMenuModel
    {
        /// <summary>
        /// 图片（用MD）
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName { get; set; }
    }
}
