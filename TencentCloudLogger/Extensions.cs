using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TencentCloudLogger
{
    public static class Extensions
    {

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }


        /// <summary>
        /// 是否为空字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNull(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }


        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string SHA1(this string srcString)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                sha1.Initialize();
                byte[] input = Encoding.UTF8.GetBytes(srcString);
                byte[] output = sha1.ComputeHash(input);
                return Convert.ToHexString(output).ToLower();
            }
        }

        /// <summary>
        /// HMAC_SHA1
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypt key</param>
        /// <returns></returns>
        public static string HMACSHA1(this string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
            {
                hmac.Initialize();
                byte[] input = Encoding.UTF8.GetBytes(srcString);
                byte[] output = hmac.ComputeHash(input);
                return Convert.ToHexString(output).ToLower();
            }
        }


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static (long start, long end) GetTencentCloudTimeStamp(this DateTimeOffset offset)
        {
            var endOffset = offset.AddHours(1);
            return (offset.ToUnixTimeSeconds(), endOffset.ToUnixTimeSeconds());
        }


        /// <summary>
        /// TencentCloud Logger依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddTencentCloudLogger(this IServiceCollection services, TCloudOption option)
        {
            services.AddLogging(builder =>
            {
                builder.AddProvider(new TCloudLoggerProvider(option));
            });
            return services;
        }
    }
}
