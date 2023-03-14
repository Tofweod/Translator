using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Translator.TranslatorApi.Impl;

namespace Translator.TranslatorApi
{
    internal static class Common
    {
        private static Dictionary<string, string> langDict = new Dictionary<string, string>() {
            {"中文","zh" },
            {"英文","en" },
            {"日文","jp" }
        };

        private static Dictionary<string, ITranslator> translatorDict = new Dictionary<string, ITranslator>()
        {
            {"ChatGPT",new ChatgptTranslator() },
            {"百度翻译",new BaiduTranslator() }
        };

        public static Dictionary<string,string> GetLangDict() { return langDict; }

        public static Dictionary<string,ITranslator> GetTranslatorDict() { return translatorDict; }

        public static int GetTranslatorIndex(string s)
        {
            for(int i = 0;i < translatorDict.Count; ++i)
            {
                if(translatorDict.ElementAt(i).Key == s)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int GetLangIndex(string s)
        {
            for (int i = 0; i < langDict.Count; ++i)
            {
                if (langDict.ElementAt(i).Value == s)
                {
                    return i;
                }
            }
            return -1;
        }

        public static string MD5IN32(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(s));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));//转化为小写的16进制
            }
            return sBuilder.ToString();

        }
    }
}
