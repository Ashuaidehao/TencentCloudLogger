using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Protobuf;
using TencentCloud.Cls;

namespace TencentCloudLogger.Http
{
    public class TCloudClient
    {
        private const string ContentType = "application/x-protobuf";

        private readonly string _secretId;
        private readonly string _secretKey;
        private readonly string _topic;


        private long lastEndTime = 0;
        private string authorizationString = "";


        private readonly HttpClient _client;


        public TCloudClient(string host, string secretId, string secretKey, string topic)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(host);
            _secretId = secretId;
            _secretKey = secretKey;
            _topic = topic;
        }

        /// <summary>
        /// 获取请求Authorization头部的授权字符串
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizationString()
        {
            var (start, end) = DateTimeOffset.Now.GetTencentCloudTimeStamp();
            if (lastEndTime > start)
            {
                return authorizationString;
            }
            var timeStr = $"{start};{end}";
            lastEndTime = end;
            var signature = Signature.GetSignature("post", "/structuredlog", timeStr, _secretKey);
            authorizationString = $"q-sign-algorithm=sha1&q-ak={_secretId}&q-sign-time={timeStr}&q-key-time={timeStr}&q-header-list=&q-url-param-list=&q-signature={signature}";
            return authorizationString;
        }



        /// <summary>
        /// 上传日志
        /// </summary>
        /// <param name="logGroupList"></param>
        /// <returns></returns>
        public async Task UploadLog(LogGroupList logGroupList)
        {
            await UploadLog(logGroupList, _topic);
        }

        /// <summary>
        /// 上传日志
        /// </summary>
        /// <param name="logGroupList"></param>
        /// <returns></returns>
        public async Task UploadLog(LogGroupList logGroupList, string topic)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"/structuredlog?topic_id={topic}");
            requestMessage.Headers.TryAddWithoutValidation("Authorization", GetAuthorizationString());
            var content = new ByteArrayContent(logGroupList.ToByteArray());
            content.Headers.Clear();
            content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            requestMessage.Content = content;

            var response = await _client.SendAsync(requestMessage);
            //var result = await response.Content.ReadAsStringAsync();
        }
    }
}
