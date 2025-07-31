using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace WpfPrism.Models
{
    /// <summary>
    /// 发布订阅 Model
    /// </summary>
    internal class MsgEvent : PubSubEvent<string>
    {
    }
}
