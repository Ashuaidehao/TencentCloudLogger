# TencentCloudLogger

## Asp.Net core 中使用方法

appsettings.json 中添加如下配置项

```js
{
  "TencentCloudLogger": {
    "Level": "Information",//记录的最低日志级别
    "ApiHost": "http://ap-shanghai.cls.tencentcs.com", //请求地址
    "SecretId": "AKID....",
    "SecretKey": "sVrw...",
    "Topic": "4807fc45-xxxx-xxxx-xxxxxxxxxxxx" //日志主题ID
  }
}
```

Startup.cs 文件中ConfigureServices方法里依赖注入

```csharp
    var option = Configuration.GetSection("TencentCloudLogger").Get<TCloudOption>();
    services.AddTencentCloudLogger(option);
```

