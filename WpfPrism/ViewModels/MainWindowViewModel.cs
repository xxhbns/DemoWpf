using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources.Extensions;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDialogAware
    {
        #region 实现接口
        public string Title => throw new NotImplementedException();

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 对话框服务
        /// </summary>
        private readonly IDialogService _dialogService;


        /// <summary>
        /// 发布订阅
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            LmList = new List<LeftMenuModel>();
            InitLeftMenu();

            NavigateCom = new DelegateCommand<LeftMenuModel>(Navigate);

            ForwardCom = new DelegateCommand(() =>
            {
                if (regionNavigationJournal != null && regionNavigationJournal.CanGoForward)
                {
                    regionNavigationJournal.GoForward();
                }
            });

            BackCom = new DelegateCommand(() =>
            {
                if (regionNavigationJournal != null && regionNavigationJournal.CanGoBack)
                {
                    regionNavigationJournal.GoBack();
                }
            });
        }

        #region 左侧菜单
        /// <summary>
        /// 左侧菜单
        /// </summary>
        private List<LeftMenuModel> _lmList;

        public List<LeftMenuModel> LmList
        {
            get { return _lmList; }
            set { _lmList = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        private void InitLeftMenu()
        {
            LmList.Add(new LeftMenuModel
            {
                Icon = "Home",
                MenuName = "首页",
                ViewName = "HomeUC"
            });
            LmList.Add(new LeftMenuModel
            {
                Icon = "NotebookOutline",
                MenuName = "待办事项",
                ViewName = "ToDoUC"
            });
            LmList.Add(new LeftMenuModel
            {
                Icon = "NotebookPlus",
                MenuName = "备忘录",
                ViewName = "MemoUC"
            });
            LmList.Add(new LeftMenuModel
            {
                Icon = "Cog",
                MenuName = "设置",
                ViewName = "SettingsUC"
            });
        }
        #endregion

        #region 区域导航
        /// <summary>
        /// 区域管理
        /// </summary>
        private readonly IRegionManager _regionManager;

        public DelegateCommand<LeftMenuModel> NavigateCom { get; set; }

        private void Navigate(LeftMenuModel model)
        {
            if (model != null && model.ViewName != null)
                _regionManager.Regions["MainViewRegion"].RequestNavigate(model.ViewName, callback => 
                {
                    regionNavigationJournal = callback.Context.NavigationService.Journal;
                });
        }

        #endregion

        #region 前进 后退
        /// <summary>
        /// 导航记录
        /// </summary>
        private IRegionNavigationJournal regionNavigationJournal;

        public DelegateCommand ForwardCom { get; private set; }
        public DelegateCommand BackCom { get; private set; }
        #endregion

    }
}
