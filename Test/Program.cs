using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.TranslatorApi;
using Translator.TranslatorApi.Impl;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string testText = "i am student";
            ITranslator translator = new BaiduTranslator();
            //ITranslator t = new ChatgptTranslator();
            //TestTranslation(t, testText);
            TestTranslation(translator, testText);
            while (true) ;
        }

        public async static void TestTranslation(ITranslator t,string s)
        {
            string res = await t.TranslateAsync(s);
            Console.WriteLine(res);
        }
    }
}
