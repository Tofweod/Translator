using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
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
using System.Windows.Interop;
using Translator.TranslatorApi;
using System.Configuration;
using System.Threading;
using System.Timers;

namespace Translator
{

    public partial class MainWindow : Window
    {

        public System.Windows.Threading.DispatcherTimer timer;//定时器

        IntPtr hwnd; // 窗口句柄

        ITranslator translator;
        string src_lang;
        string dst_lang;

        bool valid;

        public MainWindow()
        {
            InitializeComponent();
            // 颜色、字体设置
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.BackgroundColor));
            // 默认翻译选项
            translator = Common.GetTranslatorDict()[Settings.TranslatorName];
            src_lang = Settings.Src_Lang;
            dst_lang = Settings.Dst_Lang;
            valid = Settings.Valid;

            // 设置句柄、定时器
            hwnd = new WindowInteropHelper(this).Handle;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        #region 置顶函数
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        #endregion

        #region 消息钩子预定义参数
        private const int WM_DRAWCLIPBOARD = 0x308;

        private const int WM_CHANGECBCHAIN = 0x30D;

        private IntPtr mNextClipBoardViewerHWnd;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern bool ChangeClipboardChain(IntPtr HWnd, IntPtr HWndNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion

        #region 消息钩子函数
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            //挂消息钩子
            mNextClipBoardViewerHWnd = SetClipboardViewer(source.Handle);
            source.AddHook(WndProc);
        }

      


        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DRAWCLIPBOARD:
                    {
                        SendMessage(mNextClipBoardViewerHWnd, msg, wParam.ToInt32(), lParam.ToInt32());
                        //文本内容检测
                        if (System.Windows.Clipboard.ContainsText())
                        {
                            //System.Windows.Clipboard.GetText()此处有Bug
                            //String ct = System.Windows.Clipboard.GetText();
                            String ct = Clipboard.GetText();
                            source_lang.Text = "\t" + ct;
                            UpdateDestLangAsync(ct,src_lang,dst_lang);
                        }
                    }
                    break;
                case WM_CHANGECBCHAIN:
                    {
                        if (wParam == (IntPtr)mNextClipBoardViewerHWnd)
                        {
                            mNextClipBoardViewerHWnd = lParam;
                        }
                        else
                        {
                            SendMessage(mNextClipBoardViewerHWnd, msg, wParam.ToInt32(), lParam.ToInt32());
                        }
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }
        #endregion


        #region 引入穿透函数
        private const int WS_EX_TRANSPARENT = 0x20;

        private const int GWL_EXSTYLE = -20;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        #endregion



        private void MainWindow_Closing(object sender, EventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            //移除消息钩子
            ChangeClipboardChain(source.Handle, mNextClipBoardViewerHWnd);
            source.RemoveHook(WndProc);
            timer.Stop();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow_Closing(sender, e);
            Environment.Exit(0);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.WindowState != WindowState.Minimized)
            {
                SetWindowPos(hwnd, -1, 0, 0, 0, 0, 1 | 2);
            }
        }


        private async void UpdateDestLangAsync(string text, string src, string dst)
        {
            if (valid)
            {
                dest_lang.Text = "\t"+await TimeoutAfter(translator.TranslateAsync(text, src, dst),Settings.TranslateTimeout);
            }
            else
            {
                dest_lang.Text = "\t已禁止翻译";
            }
        }

        // 超时设置
        public async Task<string> TimeoutAfter(Task<string> task,int timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }
                else
                {
                   return "翻译请求超时";
                }
            }
        }

        private void Drag(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void Valid_Button_Click(object sender, RoutedEventArgs e)
        {
            if (valid)
            {
                valid = false;
                Valid.FontWeight = FontWeights.Normal;
            }
            else
            {
                valid = true;
                Valid.FontWeight = FontWeights.Bold;
            }
        }
        
        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow
            {
                Owner = this
            };
            settingsWindow.Show();
        }

        private void Font_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Background_Button_Click(object sender, RoutedEventArgs e)
        {
            var backgroundWindow = new BackgroundWindow
            {
                Owner = this
            };
            backgroundWindow.Show();
        }


        public void SetV(string s)
        {
            source_lang.Text = s;
        }

        public void SetTranslator(ITranslator t)
        {
            translator = t;
        }

        public void SetSrcLang(string s)
        {
            src_lang = s;
        }

        public void SetDstLang(string s)
        {
            dst_lang = s;
        }

        private void Penetration_Button_Click(object sender, RoutedEventArgs e)
        {
            //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(Body)).Handle;
            //SetWindowLong(hwnd, GWL_EXSTYLE,
            //    GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_TRANSPARENT);
        }
    }
}
