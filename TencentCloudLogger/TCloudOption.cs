using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TencentCloudLogger
{
    /// <summary>
    /// 日志组件配置
    /// </summary>
    public class TCloudOption
    {
        /// <summary>
        /// 日志最低级别
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// 日志请求服务地址,例如：http://cls.tencentcloudapi.com
        /// </summary>
        public string ApiHost { get; set; }

        /// <summary>
        /// SecretId
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 日志主题id
        /// </summary>
        public string Topic { get; set; }
    }
}
