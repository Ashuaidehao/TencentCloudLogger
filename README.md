# TencentCloudLogger

## 注意

1. 该项目封装的是 2017 版的 API，非最新的 3.0 版 API，官方地址：https://cloud.tencent.com/document/api/614/16873
2. 2017 版 API 官方截至 2022-1-9 给出的地址是  `<Region>.cls.tencentyun.com `，经测试容易出现503错误，可使用 `<Region>.cls.tencentcs.com` 代替。

## 配置
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

Startup.cs 文件中 ConfigureServices 方法里依赖注入

```csharp
var option = Configuration.GetSection("TencentCloudLogger").Get<TCloudOption>();
services.AddTencentCloudLogger(option);
```

> .NET 6

在 Program.cs 文件中 

```csharp
var option = builder.Configuration.GetSection("TencentCloudLogger").Get<TCloudOption>();
builder.Services.AddTencentCloudLogger(option);
```

# 使用

```csharp
private readonly ILogger _logger;

public Test(ILogger<TCloudLogger> logger)
{
    _logger = logger;
}

_logger.LogInformation("Test");
_logger.LogError("Test");
_logger.LogDebug("Test");
_logger.LogWarning("Test");
...
...
...

```