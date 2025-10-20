using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrism.Models;

namespace WpfPrism.ViewModels
{
    public partial class SettingsUCViewModel : ObservableValidator
    {
        /// <summary>
        /// 区域管理
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// 导航记录
        /// </summary>
        private IRegionNavigationJournal _regionNavigationJournal;

        public SettingsUCViewModel(IRegionManager regionManager, IRegionNavigationJournal regionNavigationJournal)
        {
            _regionManager = regionManager;
            _regionNavigationJournal = regionNavigationJournal;

            InitLeftMenu();
        }

        #region 左侧菜单
        /// <summary>
        /// 左侧菜单
        /// </summary>
        [ObservableProperty]
        private List<LeftMenuModel> _leftMenuList;

        /// <summary>
        /// 创建菜单
        /// </summary>
        private void InitLeftMenu()
        {
            LeftMenuList = [];
            LeftMenuList.Add(new LeftMenuModel
            {
                Icon = "Palette",
                MenuName = "个性化",
                ViewName = "PersonalUC"
            });
            LeftMenuList.Add(new LeftMenuModel
            {
                Icon = "Cog",
                MenuName = "系统设置",
                ViewName = "SysSetUC"
            });
            LeftMenuList.Add(new LeftMenuModel
            {
                Icon = "Information",
                MenuName = "关于更多",
                ViewName = "AboutUC"
            });
        }
        #endregion

        #region 区域导航
        //public DelegateCommand<LeftMenuModel> NavigateCommand { get; set; }

        /// <summary>
        /// ContentControl + UserControl 结合区域管理获取对应的 View
        /// </summary>
        /// <param name="model"></param>
        [RelayCommand]
        private void Navigate(LeftMenuModel model)
        {
            if (model != null && model.ViewName != null)
                _regionManager.Regions["SettingRegion"].RequestNavigate(model.ViewName);
        }
        #endregion

    }
}
