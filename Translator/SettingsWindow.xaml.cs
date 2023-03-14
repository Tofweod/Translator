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
using System.Windows.Shapes;
using Translator.SettingPages.TranslatorPages;

namespace Translator
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();       
        }

        private void General_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("pack://application:,,,/SettingPages/General_Selected_Page.xaml"));
        }

        private void ChatGPT_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("pack://application:,,,/SettingPages/TranslatorPages/ChatGPT_Selected_Page.xaml"));
        }

        private void Baidu_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("pack://application:,,,/SettingPages/TranslatorPages/Baidu_Selected_Page.xaml"));
        }

        private void Deepl_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("pack://application:,,,/SettingPages/TranslatorPages/Deepl_Selected_Page.xaml"));
        }

        private void DontClick_Selected(object sender, RoutedEventArgs e)
        {
            this.SettingFrame.Navigate(new Uri("pack://application:,,,/SettingPages/DontClick_Selected_Page.xaml"));
        }

        ~SettingsWindow()
        {
            Settings.SaveAppConfig();
        }
    }
}
