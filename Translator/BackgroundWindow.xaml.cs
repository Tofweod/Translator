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

namespace Translator
{
    /// <summary>
    /// BackgroundSettings.xaml 的交互逻辑
    /// </summary>
    public partial class BackgroundWindow : Window

    {
        public BackgroundWindow()
        {
            InitializeComponent();
        }

        private void BackgroundColor_Changed(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Background = ColorPicker.SelectedBrush;
            Settings.BackgroundColor = ColorPicker.SelectedBrush.ToString();
        }

        ~BackgroundWindow()
        {
            Configuration config = Settings.GetAppConfig();
            config.AppSettings.Settings["BackgroundColor"].Value = Settings.BackgroundColor;
            Settings.SaveAppConfig();
        }
    }
}
