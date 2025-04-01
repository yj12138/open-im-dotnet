using IMDemo;
using IMDemo.Chat;
using YamlDotNet.Serialization;

var arguments = new Dictionary<string, string>();
for (int i = 0; i < args.Length; i += 2)
{
    if (i + 1 < args.Length && args[i].StartsWith("--"))
    {
        arguments[args[i].TrimStart('-')] = args[i + 1];
    }
}

var deserializer = new DeserializerBuilder().Build();
var configFilePath = "config.yaml";
var configContent = File.ReadAllText(configFilePath);
var config = deserializer.Deserialize<Config>(configContent);
ChatMgr.Instance.config = config;
var app = new DemoApplication("IMDemo", 1000, 800, config);

if (arguments.Count > 0)
{
    app.OnLoadCallBack = () =>
    {
        if (arguments.ContainsKey("uid") && arguments.ContainsKey("token"))
        {
            var uid = arguments["uid"];
            var token = arguments["token"];
            User.TryLogin(uid, token);
        }
    };
}

app.Run();







