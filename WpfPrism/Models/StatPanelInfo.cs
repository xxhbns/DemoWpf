using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrism.Models
{
    /// <summary>
    /// 首页统计面板
    /// </summary>
    public partial class StatPanelInfo : ObservableValidator
    {
        /// <summary>
        /// 统计项图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 统计项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 统计结果
        /// </summary>
        [ObservableProperty]
        private string _result;
        //public string Result 
        //{
        //    get { return _result; }
        //    set 
        //    {
        //        _result = value;
        //        RaisePropertyChanged();
        //    }
        //}

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// 点击跳转的 界面名称
        /// </summary>
        public string ViewName { get; set; }

        public string Hand 
        {
            get 
            {
                if (ItemName == "完成比例")
                    return "";
                else
                    return "Hand";
            }
        }
    }
}
