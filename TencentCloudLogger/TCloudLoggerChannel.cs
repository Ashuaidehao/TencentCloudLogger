using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TencentCloud.Cls;
using TencentCloudLogger.Http;

namespace TencentCloudLogger
{
    public class TCloudLoggerChannel
    {
        private static string IP = Extensions.GetLocalIPAddress();

        private readonly Channel<Log> _channel = Channel.CreateUnbounded<Log>();
        private readonly TCloudClient _client;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public TCloudLoggerChannel(TCloudOption option):this(new TCloudClient(option.ApiHost, option.SecretId, option.SecretKey, option.Topic))
        {
        }
        public TCloudLoggerChannel(TCloudClient client)
        {
            _client = client;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async void WriteAsync(Log log)
        {
            await _channel.Writer.WriteAsync(log);
        }

        public async void Start()
        {
            var reader = _channel.Reader;
            while (await reader.WaitToReadAsync(_cancellationTokenSource.Token))
            {
                var logs = ReadBatch(reader);
                if (logs == null) continue;
                await _client.UploadLog(logs);
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }


        private static LogGroupList ReadBatch(ChannelReader<Log> reader, int count = 100)
        {
            var originCount = count;
            var logGroup = new LogGroup();
            logGroup.Source = IP;
            while (count > 0 && reader.TryRead(out var log))
            {
                logGroup.Logs.Add(log);
                count--;
            }
            if (originCount == count)
            {
                return null;
            }
            var result = new LogGroupList();
            result.LogGroupList_.Add(logGroup);
            return result;
        }
    }
}
