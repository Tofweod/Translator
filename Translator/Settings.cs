using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

namespace Translator
{
    internal static class Settings
    {
        public static int TranslateTimeout { get; set; } = int.Parse(ConfigurationManager.AppSettings["TranslateTimeout"]);

        public static string BaiduGlossary { get; set; } = ConfigurationManager.AppSettings["BaiduGlossary"];

        public static string BackgroundColor { get; set; } = ConfigurationManager.AppSettings["BackgroundColor"];

        public static string TranslatorName { get; set; } = ConfigurationManager.AppSettings["Translator"];

        public static string Src_Lang { get; set; } = ConfigurationManager.AppSettings["Src_Lang"];

        public static string Dst_Lang { get;set; } = ConfigurationManager.AppSettings["Dst_Lang"];

        public static bool Valid { get; set; } = true;

        public static string BaiduAppId { get; set; } = ConfigurationManager.AppSettings["BaiduAppId"];

        public static string BaiduKey { get; set; } = ConfigurationManager.AppSettings["BaiduKey"];

        public static string ChatgptKey { get; set; } = ConfigurationManager.AppSettings["ChatgptKey"];

        private static Configuration AppConfig = null;

        public static Configuration GetAppConfig()
        {
            if(AppConfig == null)
            {
                try
                {
                    AppConfig = ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                }
                catch (ConfigurationErrorsException cee)
                {
                    MessageBox.Show(cee.Message);
                    Environment.Exit(1);
                }
            }
            return AppConfig;
        }

        public static void SaveAppConfig()
        {
            if(AppConfig != null)
            {
                // 保存并刷新
                AppConfig.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
