using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TencentCloudLogger
{
    public class TCloudLoggerProvider : ILoggerProvider
    {

        private readonly TCloudOption _option;
        private readonly TCloudLoggerChannel _channel;
        public TCloudLoggerProvider(IOptions<TCloudOption> option) : this(option.Value)
        {
        }

        public TCloudLoggerProvider(TCloudOption option)
        {
            if (option.ApiHost.IsNull() || option.SecretId.IsNull() || option.SecretKey.IsNull() || option.Topic.IsNull())
            {
                throw new ArgumentNullException(nameof(option), "ApiHost,SecretId,SecretKey,Topic must not be null!");
            }
            _option = option;
            _channel = new TCloudLoggerChannel(option);
            _channel.Start();
        }

        public void Dispose()
        {
            _channel?.Stop();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TCloudLogger(_channel, _option, categoryName);
        }
    }
}
