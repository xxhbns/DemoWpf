using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.HttpClients.Interfaces;
using WpfPrism.Models;

namespace WpfPrism.HttpClients.Services
{
    /// <summary>
    /// 登录用户信息实现类
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private UserInfo _currentUser = new();

        public string Name => _currentUser.Name;

        public string Account => _currentUser.Account;

        public void SetUser(UserInfo userInfo)
        {
            _currentUser = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
        }

        public void Clear()
        {
            _currentUser = new();
        }

    }
}
