using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Translator.TranslatorApi.Impl
{
    /// <summary>
    /// 由于WPF使用的.net框架版本最高为4.7.2，Deepl官方提供的.net类库所支持框架版本为5.0，因此使用不了
    /// 同时，Deepl并不支持中文的术语库，也无法使用中国信用卡申请api
    /// 所幸Deepl推出了免费的应用程序，可完成与本程序相同的功能
    /// 综上，此接口暂时不考虑实现
    /// </summary>
    public class DeeplTranslator : ITranslator
    {
        public Task<string> TranslateAsync(string text, string src, string dst)
        {
            // todo
            return null;
        }

        public void CheckUpdate() { }

    }
}
