using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TencentCloud.Cls;

namespace TencentCloudLogger
{
    public class TCloudLogger : ILogger
    {
        private readonly TCloudLoggerChannel _channel;
        private readonly TCloudOption _config;
        private readonly string _name;

        /// <summary>
        /// 获取自定义Logger实例
        /// </summary>
        /// <param name="option"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TCloudLogger CreateLogger(TCloudOption option, string name = "default_logger")
        {
            var channel = new TCloudLoggerChannel(option);
            var logger = new TCloudLogger(channel, option, name);
            return logger;
        }

        public TCloudLogger(TCloudLoggerChannel channel, TCloudOption config, string name)
        {
            _channel = channel;
            _config = config;
            _name = name;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            var msg = formatter != null ? formatter(state, exception) : state.ToString();
            var log = new Log();
            log.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            log.Contents.Add(new Log.Types.Content() { Key = "Message", Value = msg });
            log.Contents.Add(new Log.Types.Content() { Key = "EventId", Value = eventId.ToString() });
            log.Contents.Add(new Log.Types.Content() { Key = "Logger", Value = _name });
            log.Contents.Add(new Log.Types.Content() { Key = "Level", Value = logLevel.ToString() });
            if (exception != null)
            {
                log.Contents.Add(new Log.Types.Content() { Key = "Exception", Value = exception.ToString() });
            }
            _channel.WriteAsync(log);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (_config != null)
            {
                return logLevel >= _config.Level;
            }
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullLogScope.Instance;
        }

        public class NullLogScope : IDisposable
        {

            public static NullLogScope Instance = new NullLogScope();
            public void Dispose()
            {
            }
        }
    }
}
