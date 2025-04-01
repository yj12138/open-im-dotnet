using IMDemo.Chat;
using IMDemo.UI;
using OpenTK.Windowing.Common;
using IMDemo.Core;

namespace IMDemo
{
    public class DemoApplication : Application
    {
        public static Config Config;
        public Action OnLoadCallBack;
        public DemoApplication(string title, int width, int height, Config config) : base(title, width, height)
        {
            Config = config;
            Debug.Log(Config.TestAccounts.Length);
            foreach (var info in config.TestAccounts)
            {
                AddMenuItem("Start/Login/" + info.UserId, () =>
                {
                    User.TryLogin(info.UserId, info.Token);
                });
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            if (!ChatMgr.Instance.InitSDK())
            {
                Debug.Log("InitSDK error");
                Close();
                return;
            }
            if (OnLoadCallBack != null)
            {
                OnLoadCallBack();
            }
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            ChatMgr.Instance.UnInitSDK();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }
        protected override void OnGUI()
        {
            StatusBar.Draw();
        }
    }
}