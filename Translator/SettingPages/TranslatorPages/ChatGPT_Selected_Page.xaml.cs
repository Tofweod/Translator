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
    /// ChatGPT_Selected_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ChatGPT_Selected_Page : Page
    {
        public ChatGPT_Selected_Page()
        {
            InitializeComponent();

            key.Text = Settings.ChatgptKey;
        }

        private void Key_TextChanged(object sender, TextChangedEventArgs e)
        {
            Configuration config = Settings.GetAppConfig();
            if (string.IsNullOrEmpty(Settings.ChatgptKey))
            {
                config.AppSettings.Settings.Add("ChatgptKey", key.Text);
            }
            else
            {
                config.AppSettings.Settings["ChatgptKey"].Value = key.Text; 
            }
            Settings.ChatgptKey = key.Text;
            UpdateChanges();
        }

        private void UpdateChanges()
        {
            Common.GetTranslatorDict()["ChatGPT"].CheckUpdate();
        }
    }
}
