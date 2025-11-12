using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Models.DTO;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.Helpers;
using WpfPrism.HttpClients;
using WpfPrism.HttpClients.Interfaces;
using WpfPrism.HttpClients.Services;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    public partial class LoginUCViewModel : ObservableValidator, IDialogAware
    {
        public string Title { get; set; } = "登录";//标题

        /// <summary>
        /// http
        /// </summary>
        //private readonly HttpRestClient _httpRestClient;

        /// <summary>
        /// 消息通知（发布订阅）
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// 登录用户信息管理
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// 用户 api 接口管理
        /// </summary>
        private readonly UserServices _userServices;

        public event Action<IDialogResult> RequestClose;

        public LoginUCViewModel(
            IEventAggregator eventAggregator, 
            UserServices userServices, 
            ICurrentUserService currentUserService
            )
        {
            UsersReq = new UsersReq();

            //请求Client
            //_httpRestClient = httpRestClient;

            _eventAggregator = eventAggregator;
            _userServices = userServices;
            _currentUserService = currentUserService;

            //LoginCmd = new DelegateCommand(Login);
            //ChangedCmd = new DelegateCommand(Changed);
            //RegCmd = new DelegateCommand(Reg);
            //LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        #region 显示注册界面 返回登录界面
        [ObservableProperty]
        private int _selectIndex;

        //public DelegateCommand ChangedCmd { get; set; }

        [RelayCommand]
        private void Changed()
        {
            SelectIndex = SelectIndex == 0 ? 1 : 0;
        }

        #endregion

        #region 登录
        /// <summary>
        /// 账号
        /// </summary>
        [ObservableProperty]
        private string _account;

        /// <summary>
        /// 密码
        /// </summary>
        [ObservableProperty]
        private string _myPwd;

        //public DelegateCommand LoginCmd { get; set; }

        //public IAsyncRelayCommand LoginCommand { get; }

        /// <summary>
        /// 登录
        /// </summary>
        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(MyPwd))
                {
                    _eventAggregator.GetEvent<MsgEvent>().Publish("信息不全");
                    return;
                }

                ApiResponse apiResponse = await _userServices.UserLoginAsync(Account, MyPwd);

                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                if (apiResponse.IsSuccess)
                {
                    var usersReq = JsonConvert.DeserializeObject<UsersReq>(apiResponse.Parameter.ToString());
                    DialogParameters paras = new()
                    {
                        { "LoginName", usersReq.Name }
                    };

                    _currentUserService.SetUser(new UserInfo 
                    {
                        Name = usersReq.Name,
                        Account = usersReq.Account,
                    });

                    Log.Information("用户登录成功！");

                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, paras));
                }
                else
                {
                    MyPwd = "";
                }
            }
            catch (Exception ex)
            {
                // 记录并显示异常信息
                _eventAggregator.GetEvent<MsgEvent>().Publish($"登录失败: {ex.Message}");
                MyPwd = "";
            }
        }
        #endregion

        #region 注册
        [ObservableProperty]
        private UsersReq _usersReq;

        //public DelegateCommand RegCmd { set; get; }

        [RelayCommand]
        private async Task Reg()
        {
            try
            {
                //数据校验
                if (string.IsNullOrEmpty(UsersReq.Name) || string.IsNullOrEmpty(UsersReq.Account) || string.IsNullOrEmpty(UsersReq.Password) || string.IsNullOrEmpty(UsersReq.ConfirmPwd))
                {
                    //发布消息
                    _eventAggregator.GetEvent<MsgEvent>().Publish("信息不全");
                    return;
                }
                if (UsersReq.Password != UsersReq.ConfirmPwd)
                {
                    _eventAggregator.GetEvent<MsgEvent>().Publish("两次密码输入不一致");
                    return;
                }

                //调用Api
                ApiResponse apiResponse = await _userServices.UserRegAsync(UsersReq);

                _eventAggregator.GetEvent<MsgEvent>().Publish(apiResponse.Msg);
                if (apiResponse.IsSuccess)
                {
                    SelectIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _eventAggregator.GetEvent<MsgEvent>().Publish($"注册失败: {ex.Message}");
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
