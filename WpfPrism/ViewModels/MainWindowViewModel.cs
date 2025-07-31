using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace WpfPrism.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// 导航记录
        /// </summary>
        private IRegionNavigationJournal regionNavigationJournal;

        /// <summary>
        /// 区域管理
        /// </summary>
        private readonly IRegionManager _regionManager;

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


        }

    }
}
