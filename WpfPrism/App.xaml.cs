using System.Configuration;
using System.Data;
using System.Windows;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using WpfPrism.HttpClients;
using WpfPrism.ViewModels;
using WpfPrism.Views;

namespace WpfPrism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// 设置启动页
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 注入服务 <see cref="IContainerRegistry"/> <see langword="bool"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// 注入服务是指在应用程序启动时，将需要的服务或组件注册到依赖注入容器中，以便在应用程序的其他部分可以使用这些服务。
        /// </para>
        /// </remarks>
        /// <example>
        /// Csharp用法
        /// <code language="csharp">
        /// <![CDATA[
        /// containerRegistry.RegisterDialog<T, TViewModel>();
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="containerRegistry"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ///注入
            containerRegistry.RegisterDialog<LoginUC, LoginUCViewModel>();
            containerRegistry.RegisterDialog<MainWindow, MainWindowViewModel>();

            containerRegistry.RegisterForNavigation<HomeUC, HomeUCViewModel>();
            containerRegistry.RegisterForNavigation<ToDoUC, ToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCViewModel>();
            containerRegistry.RegisterForNavigation<SettingsUC, SettingsUCViewModel>();

            containerRegistry.RegisterForNavigation<PersonalUC, PersonalUCViewModel>();
            containerRegistry.RegisterForNavigation<SysSetUC>();
            containerRegistry.RegisterForNavigation<AboutUC>();

            //请求
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));

        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInitialized()
        {
            //打开对话框
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                var vm = Current.MainWindow.DataContext as MainWindowViewModel;
                if (vm != null)
                {
                    if (callback.Parameters.TryGetValue<string>("LoginName", out var name))
                    {
                        vm.NavigateDefaultCommand.Execute(name);
                    }
                }
            });

            base.OnInitialized();
        }

        #region 通过引用实现模块化
        ///// <summary>
        ///// 通过引用实现模块化
        ///// </summary>
        ///// <param name="moduleCatalog"></param>
        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //    moduleCatalog.AddModule<ModuleAProfile>();
        //    moduleCatalog.AddModule<ModuleBProfile>();
        //    base.ConfigureModuleCatalog(moduleCatalog);
        //}
        #endregion

        #region 通过路径实现模块化
        ///// <summary>
        ///// 通过路径实现模块化
        ///// </summary>
        ///// <returns></returns>
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        //}
        #endregion
    }

}
