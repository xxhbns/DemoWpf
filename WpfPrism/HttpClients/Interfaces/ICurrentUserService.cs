using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.Models;

namespace WpfPrism.HttpClients.Interfaces
{
    /// <summary>
    /// 登录用户信息接口
    /// </summary>
    public interface ICurrentUserService
    {
        string Name { get; }

        string Account { get; }

        void SetUser(UserInfo userInfo);

        void Clear();


    }
}
