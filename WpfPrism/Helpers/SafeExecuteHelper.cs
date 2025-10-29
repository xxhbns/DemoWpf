using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPrism.Helpers
{
    public class SafeExecuteHelper
    {
        /// <summary>
        /// 安全执行辅助方法
        /// </summary>
        /// <param name="asyncAction">需要执行的方法</param>
        /// <returns></returns>
        public static async Task SafeExecuteAsync(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction();
            }
            catch (Exception ex)
            {
                // 在UI线程显示错误
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    // 显示错误信息，而不是让应用崩溃
                    MessageBox.Show("操作失败，请稍后重试", "错误",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}
