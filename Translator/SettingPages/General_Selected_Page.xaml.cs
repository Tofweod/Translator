using HandyControl.Tools.Extension;
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

namespace Translator.SettingPages
{
    /// <summary>
    /// Lang_Selected_Page.xaml 的交互逻辑
    /// </summary>
    public partial class General_Selected_Page : Page
    {
 
        private List<string> LangList;

        public General_Selected_Page()
        {
            InitializeComponent();
            LangList = Common.GetLangDict().Keys.ToList();
            SrcLangCombox.ItemsSource = LangList;
            DstLangCombox.ItemsSource = LangList;
            TranslatorCombox.ItemsSource = Common.GetTranslatorDict().Keys.ToList();


            TranslatorCombox.SelectedIndex = Common.GetTranslatorIndex(Settings.TranslatorName);
            TranslateTimeout.Text = Settings.TranslateTimeout.ToString();
            SrcLangCombox.SelectedIndex = Common.GetLangIndex(Settings.Src_Lang);
            DstLangCombox.SelectedIndex = Common.GetLangIndex(Settings.Dst_Lang);
        }

        private void General_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = Settings.GetAppConfig();
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            string TranslatorName = (string)TranslatorCombox.SelectedValue;
            if (!string.IsNullOrEmpty(TranslatorName))
            {
                Settings.TranslatorName = TranslatorName;
                mainWindow.SetTranslator(Common.GetTranslatorDict()[Settings.TranslatorName]);
                //　写入config中
                config.AppSettings.Settings["Translator"].Value = TranslatorName;
            }
            string translateTimeout = TranslateTimeout.Text;
            if (!string.IsNullOrEmpty(translateTimeout))
            {
                int timeout;
                try
                {
                   timeout  = int.Parse(translateTimeout);
                }
                catch
                {
                    MessageBox.Show("输入数字格式有误");
                    return;
                }
                Settings.TranslateTimeout = timeout;
                mainWindow.SetTimeOut(timeout);
                config.AppSettings.Settings["TranslateTimeout"].Value = timeout.ToString();
            }
            string src = (string)SrcLangCombox.SelectedValue;
            string dst = (string)DstLangCombox.SelectedValue;
            if (src == dst) // 可能为null
            {
                MessageBox.Show("源语言与目标语言相同");
                return;
            }
            if (!string.IsNullOrEmpty(src))
            {
                Settings.Src_Lang = Common.GetLangDict()[src];
                mainWindow.SetSrcLang(Common.GetLangDict()[src]);
                config.AppSettings.Settings["Src_Lang"].Value = Common.GetLangDict()[src];
            }
            if(!string.IsNullOrEmpty(dst))
            {
                Settings.Dst_Lang = Common.GetLangDict()[dst];
                mainWindow.SetDstLang(Common.GetLangDict()[dst]);
                config.AppSettings.Settings["Dst_Lang"].Value = Common.GetLangDict()[dst];
            }
            MessageBox.Show("设置成功");
        }
    }
}
