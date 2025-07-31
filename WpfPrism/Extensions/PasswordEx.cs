using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace WpfPrism.Extensions
{
    public class PasswordEx
    {
        public static string GetMyPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(MyPwdProperty);
        }

        public static void SetMyPwd(DependencyObject obj, string value)
        {
            obj.SetValue(MyPwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyPwd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPwdProperty =
            DependencyProperty.RegisterAttached("MyPwd", typeof(string), typeof(PasswordEx), new PropertyMetadata("", OnPwdChange));

        /// <summary>
        /// 自定义附加属性发生变化，改变Password属性
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void OnPwdChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pwdbx = (PasswordBox)d;
            var pwd = e.NewValue as string;
            if (pwdbx != null && pwdbx.Password != pwd)
            {
                pwdbx.Password = pwd;
            }
        }

    }

    /// <summary>
    /// Password行为发生变化，自定义附加属性跟随变化
    /// </summary>
    public class PasswordBev : Behavior<PasswordBox>
    {
        /// <summary>
        /// 附加 注入事件
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPwdChanged;
        }

        private void OnPwdChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            string pwd = PasswordEx.GetMyPwd(pb);
            if (pb != null && pb.Password != pwd)
            {
                PasswordEx.SetMyPwd(pb, pb.Password);
            }
        }

        /// <summary>
        /// 销毁 移除事件
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPwdChanged;
        }
    }
}
