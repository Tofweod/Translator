using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Expression.Shapes;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Translator.TranslatorApi.Impl
{
    public class ChatgptTranslator : ITranslator
    {
        string apiKey = Settings.ChatgptKey;
        const string url = "https://api.openai.com/v1/chat/completions";

        public void CheckUpdate()
        {
            apiKey = Settings.ChatgptKey;
        }

        public async Task<string> TranslateAsync(string text, string src, string dst)
        {
            return await DoTranslation(text,src,dst);
        }

        public void SetApiKey(string s)
        {
            apiKey = s; 
        }


        private async Task<string> DoTranslation(string text,string src,string dst)
        {
            var client = new HttpClient();

            var data = new
            {
                model = "gpt-3.5-turbo",
                messages = new Message[] {new Message{role="system",content="Please translate "+src+" into "+dst}, // 此处role和content对应api中参数，不可修改
                                                                                new Message { role = "user", content = text } }
            };
            var requestBody = JsonConvert.SerializeObject(data);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(url, content);
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
            var getStr = response.Content.ReadAsStringAsync().Result;


            try
            {
                var res = (Result)JsonConvert.DeserializeObject<Result>(getStr);
                return res.Choices[0].Message.content.Trim();
            }
            catch
            {
                try
                {
                    // 使用Error_Message类捕获错误json，并输出错误信息
                    var err_res = (Error_Message)JsonConvert.DeserializeObject<Error_Message>(getStr);
                    return err_res.error.Message;
                }
                catch
                {
                    return "Unknown Error";
                }
            }
        }

        public async void TestChatgpt(string s)
        {
            var client = new HttpClient();
            var data = new
            {
                model = "gpt-3.5-turbo",
                messages = new Message[] {new Message{role="user",content=s}, // 此处role和content对应api中参数，不可修改
                                                                                }
            };
            var requestBody = JsonConvert.SerializeObject(data);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }


        //辅助嵌套类

       private class Message
        {
            public string role { get; set; }
            public string content { get; set; }

        }

        private class Result
        {
            internal class Usage
            {
                public int Prompt_tokens { get; set; }
                public int Completion_tokens { get; set; }
                public int Total_tokens { get; set; }
            }
            internal class Choice
            {
                public Message Message { get; set; }
                public string Finish_reason { get; set; }
                public int Index { get; set; }
            }
            public string Id { get; set; }
            public string Object { get; set; }
            public long Created { get; set; }
            public string Model { get; set; }
            public Usage usage { get; set; }
            public Choice[] Choices { get; set; }

        }


        private class Error_Message
        {
            internal class Error
            {
                public string Message { get; set; }
                public string Type { get; set; }
                public string Param { get; set; }
                public string Code { get; set; }
            }
            public Error error { get; set; }
        }
    }
}

