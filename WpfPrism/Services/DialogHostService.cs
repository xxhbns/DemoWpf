using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPrism.Services
{
    public class DialogHostService : DialogService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters? parameters, string dialogHostName = "RootDialog")
        {
            parameters ??= new DialogParameters();

            //从容器当中去除弹窗的实例
            var content = _containerExtension.Resolve<object>(name);

            //验证实例的有效性
            if (content is not FrameworkElement dialogContent) throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null) ViewModelLocator.SetAutoWireViewModel(view, true);

            if (dialogContent.DataContext is not IDialogHostAwre viewModel) throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if (viewModel is IDialogHostAwre awre)
                {
                    awre.OnDialogOpening(parameters);
                }
                eventArgs.Session.UpdateContent(content);
            };

            return (IDialogResult)await DialogHost.Show(dialogContent, dialogHostName, eventHandler);
        }
    }
}
