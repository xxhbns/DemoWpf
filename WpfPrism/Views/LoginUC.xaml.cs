using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Events;
using WpfPrism.Models;

namespace WpfPrism.Views
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : UserControl
    {
        /// <summary>
        /// 发布订阅
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public LoginUC(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MsgEvent>().Subscribe(Sub);
        }

        /// <summary>
        /// 订阅后执行的任务
        /// </summary>
        /// <param name="obj"></param>
        private void Sub(string obj)
        {
            RegLoginBar.MessageQueue.Enqueue(obj);
        }
    }
}
