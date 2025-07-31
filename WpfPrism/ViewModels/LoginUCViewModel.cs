using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using WpfPrism.Helpers;
using WpfPrism.HttpClients;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    internal class LoginUCViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "登录";//标题

        private readonly HttpRestClient _httpRestClient;

        private readonly IEventAggregator _eventAggregator;

        public event Action<IDialogResult> RequestClose;

        public LoginUCViewModel(HttpRestClient httpRestClient, IEventAggregator eventAggregator)
        {

            LoginCmd = new DelegateCommand(Login);
            ChangedCmd = new DelegateCommand(Changed);
            RegCmd = new DelegateCommand(Reg);
            UsersReq = new UsersReq();

            //请求Client
            _httpRestClient = httpRestClient;

            _eventAggregator = eventAggregator;
        }

        #region 显示注册界面 返回登录界面
        private int _selectIndex;

        public int SelectIndex
        {
            get { return _selectIndex; }
            set { _selectIndex = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ChangedCmd { get; set; }

        private void Changed()
        {
            SelectIndex = SelectIndex == 0 ? 1 : 0;
        }

        #endregion

        #region 登录
        /// <summary>
        /// 账号
        /// </summary>
        private string _account;

        public string Account
        {
            get { return _account; }
            set { _account = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string _myPwd;

        public string MyPwd
        {
            get { return _myPwd; }
            set { _myPwd = value; RaisePropertyChanged(); }
        }

        public DelegateCommand LoginCmd { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(MyPwd))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("信息不全");
                return;
            }

            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;

            MyPwd = MD5Helper.GetMD5(MyPwd);
            apiRequest.Route = $"Users/Login?account={Account}&password={MyPwd}";

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                MyPwd = "";
            }

        }

        #endregion

        #region 注册
        private UsersReq _usersreq;

        public UsersReq UsersReq
        {
            get { return _usersreq; }
            set { _usersreq = value; RaisePropertyChanged(); }
        }

        public DelegateCommand RegCmd { set; get; }

        private void Reg()
        {
            //数据校验
            if (string.IsNullOrEmpty(UsersReq.Name) || string.IsNullOrEmpty(UsersReq.Account) || string.IsNullOrEmpty(UsersReq.Password) || string.IsNullOrEmpty(UsersReq.ConfirmPwd))
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("信息不全");
                return;
            }
            if (UsersReq.Password != UsersReq.ConfirmPwd)
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish("两次密码输入不一致");
                return;
            }

            //调用Api
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Route = "Users/Reg";

            //对密码进行处理
            UsersReq.Password = MD5Helper.GetMD5(UsersReq.Password);

            apiRequest.Parameters = UsersReq;

            ApiResponse apiResponse = _httpRestClient.Execute(apiRequest);
            _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
            if (apiResponse.IsSuccess)
            {
                SelectIndex = 0;
            }
        }

        #endregion

        #region 对话框
        /// <summary>
        /// 是否关闭
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void OnDialogClosed()
        {

        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #endregion
    }
}
