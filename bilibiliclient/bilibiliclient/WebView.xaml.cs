using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bilibiliclient
{
    /// <summary>
    /// WebView.xaml 的交互逻辑
    /// </summary>
    public partial class WebView : UserControl
    {
        public static readonly string DataBasePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "bilibiliclient");
        public const string SchemeName = "bilibiliclient";

        public WebView()
        {
            InitializeComponent();

            if (WebView2 == null)
            {
                return;
            }

            WebView2.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
            WebView2.NavigationCompleted += WebView2_NavigationCompleted;
            WebView2.NavigationStarting += WebView2_NavigationStarting;
            WebView2.WebMessageReceived += WebView2_WebMessageReceived;
        }

        protected override async void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            // 修改userdatafolder,防止部分目录权限导致无法写入
            //var webView2Environment = await CoreWebView2Environment.CreateAsync(null, DataBasePath);
            //await WebView2.EnsureCoreWebView2Async(webView2Environment);
        }


        private void WebView2_WebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            var msg = e.WebMessageAsJson;
            if (string.IsNullOrEmpty(msg)) return;

            //chrome.webview.postMessage('hello world/0')
        }


        Action LoadAction;

        public void RegLoadCallback(Action _action)
        {
            LoadAction = _action;
        }

        private void WebView2_NavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            LoadAction?.Invoke();
        }

        private void WebView2_CoreWebView2InitializationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            // set settings
            var settings = WebView2.CoreWebView2.Settings;
            settings.IsStatusBarEnabled = false;
            settings.AreDefaultContextMenusEnabled = false;

            // set cookie
            //var cmer = WebView2.CoreWebView2.CookieManager;

            //string domain = SDK.Config.ConfigInfo.ServerUrl.Replace("http://", "").Replace("https://", "").Split('/').FirstOrDefault();
            //var cookie = cmer.CreateCookie("lms_token", SDK.CacheCenter.Token, domain, null);
            //cmer.AddOrUpdateCookie(cookie);
        }


        private void WebView2_NavigationStarting(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            var url = new Uri(e.Uri);
            //if (url.Scheme == SchemeName)
            //{
            //    SDK.CacheCenter.UnCommand(e.Uri);
            //    e.Cancel = true;
            //}
        }

        public void GoUrl(string url)
        {
            WebView2.Source = new Uri(url);
        }

        public void Reload()
        {
            WebView2?.Reload();
        }

        public void ExeJS(string str)
        {
            WebView2?.CoreWebView2.ExecuteScriptAsync(str);
        }

        public void DisposeBrowser()
        {
            WebView2?.Dispose();
        }
    }
}
