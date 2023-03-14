using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.TranslatorApi
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string text,string src,string dst);
    }
}

