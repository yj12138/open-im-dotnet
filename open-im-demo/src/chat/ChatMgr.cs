using OpenIM.IMSDK;
using IMDemo.Data;
using Newtonsoft.Json;
using System.Text;
using IMDemo.Core;

namespace IMDemo.Chat
{
    public class ChatMgr
    {
        public static DemoApplication Application;
        private static ChatMgr _instance;
        public static ChatMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChatMgr();
                }
                return _instance;
            }
        }
        public User currentUser;
        ConnListener connListener;
        private ChatMgr()
        {
            connListener = new ConnListener();
        }
        public static OpenIM.IMSDK.PlatformID PlatformID
        {
            get
            {
#if WINDOWS
                return OpenIM.IMSDK.PlatformID.WindowsPlatformID;
#elif LINUX
                return OpenIM.IMSDK.PlatformID.LinuxPlatformID;
#elif MAC
                return OpenIM.IMSDK.PlatformID.OSXPlatformID;
#endif
            }
        }
        public bool InitSDK()
        {
            var config = new IMConfig()
            {
                PlatformID = (int)PlatformID,
                ApiAddr = Application.Config.APIAddr,
                WsAddr = Application.Config.WsAddr,
                DataDir = Path.Combine(AppContext.BaseDirectory, Application.Config.DataDir),
                LogLevel = Application.Config.LogLevel,
                IsLogStandardOutput = Application.Config.IsLogStandardOutput,
                LogFilePath = Path.Combine(AppContext.BaseDirectory, Application.Config.LogFilePath),
                IsExternalExtensions = Application.Config.IsExternalExtensions,
            };

            return IMSDK.GetInstance().InitSDK(config, connListener);
        }
        public void UnInitSDK()
        {
            IMSDK.GetInstance().UnInitSDK();
        }

        public string GetConnStatus()
        {
            return connListener.connectStatus.ToString();
        }

        async void RefreshToken(string userId)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var url = string.Format("{0}{1}", Application.Config.APIAddr, "/auth/get_user_token");
                    var userTokenReq = new UserTokenReq()
                    {
                        secret = "openIM123",
                        platformID = (int)ChatMgr.PlatformID,
                        userID = userId,
                    };
                    var postData = JsonConvert.SerializeObject(userTokenReq);
                    httpClient.DefaultRequestHeaders.Add("operationID", "111111");
                    httpClient.DefaultRequestHeaders.Add("token", Application.Config.AdminToken);
                    HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<UserTokenRes>(jsonResponse);
                    if (res.errCode > 0)
                    {
                        Debug.Log($"Http Request Error Code :{res.errCode + ":" + res.errMsg}");
                    }
                    else
                    {
                        var token = res.data.token;
                        // TODO
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Http Request Error:{e.Message}");
                }
            }
        }
    }
}