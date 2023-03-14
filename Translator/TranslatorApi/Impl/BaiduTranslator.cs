using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Translator.TranslatorApi;

namespace Translator.TranslatorApi.Impl
{
    public class BaiduTranslator : ITranslator
    {
        // 使用百度通用翻译api
        const string url = "https://fanyi-api.baidu.com/api/trans/vip/translate?";

        string AppId = Settings.BaiduAppId;
        string Key = Settings.BaiduKey;

        // 默认翻译选项
        string Action = Settings.BaiduGlossary;

        public async Task<string> TranslateAsync(string text, string src, string dst)
        {
            StringBuilder sb = new StringBuilder();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Content-Type", "application/x-www-form-urlencoded");
            UInt32 salt = Salt();
            sb.Append(url);
            // text做url处理
            sb.Append("q="+Uri.EscapeDataString(text));
            sb.Append("&from=" + src);
            sb.Append("&to=" + dst);
            sb.Append("&appid=" + AppId);
            sb.Append("&salt=" + salt);
            sb.Append("&sign=" + Sign(text,salt));
            sb.Append("&action=" + Action);
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(sb.ToString(),null);
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
            var getStr = response.Content.ReadAsStringAsync().Result;
            // 获取正常json
            try
            {
                var res = (Result)JsonConvert.DeserializeObject<Result>(getStr);
                return res.trans_Result[0].Dst;
            }
            catch
            {
                try
                {
                    // 错误json
                    var err_res = (Error)JsonConvert.DeserializeObject<Error>(getStr);
                    return err_res.Error_msg;
                }
                catch
                {
                    return "Unknown Error";
                }
            }
        }


        private string Sign(string text,UInt32 salt)
        {
            return Common.MD5IN32(AppId + text + salt + Key);
        }


        // 生成10位随机salt
        private static UInt32 Salt()
        {

            byte[] randomBytes = new byte[4]; 
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetNonZeroBytes(randomBytes);
            return BitConverter.ToUInt32(randomBytes, 0);
        }


        // 辅助嵌套类
        class Result
        {
            internal class Trans_Result
            {
                public string Src { get; set; }
                public string Dst { get; set; }
            }
            public string From { get; set; }
            public string To { get; set; }
            public Trans_Result[] trans_Result { get; set;}
        }

        class Error
        {
            public string Error_code { get; set; }
            public string Error_msg { get; set; }
        }
    }
}
