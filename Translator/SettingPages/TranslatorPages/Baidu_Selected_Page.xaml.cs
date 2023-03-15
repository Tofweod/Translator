using System;
using System.Collections.Generic;
using System.Configuration;
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
using Translator.TranslatorApi;

namespace Translator.SettingPages.TranslatorPages
{
    /// <summary>
    /// Baidu_Selected_Page.xaml 的交互逻辑
    /// </summary>
    public partial class Baidu_Selected_Page : Page
    {
        public Baidu_Selected_Page()
        {
            InitializeComponent();

            appID.Text = Settings.BaiduAppId;
            key.Text = Settings.BaiduKey; 
        }

        private void AppID_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration config = Settings.GetAppConfig();
            if (string.IsNullOrEmpty(Settings.BaiduAppId))
            {
                config.AppSettings.Settings.Add("BaiduAppId", appID.Text);
            }
            else
            {
                config.AppSettings.Settings["BaiduAppId"].Value = appID.Text;
            }
            Settings.BaiduAppId = appID.Text;
            UpdateChanges();
        }

        private void Key_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration config = Settings.GetAppConfig();
            if (string.IsNullOrEmpty(Settings.BaiduKey))
            {
                config.AppSettings.Settings.Add("BaiduKey", key.Text);
            }
            else
            {
                config.AppSettings.Settings["BaiduKey"].Value = key.Text;
            }
            Settings.BaiduKey = key.Text;
            UpdateChanges();
        }

        private void UpdateChanges()
        {
            Common.GetTranslatorDict()["百度翻译"].CheckUpdate();
        }
    }
}
