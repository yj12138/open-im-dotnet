using IMDemo.Chat;
using IMDemo.Core;
using IMDemo.Core.UI;

namespace IMDemo.UI
{
    public static class MainMenu
    {
        [MenuItem("Help/Official WebSite")]
        public static void OfficialWebSite()
        {
            Application.OpenUrl(DemoApplication.Config.OfficialWebSite);
        }

        [MenuItem("Help/Doc WebSite")]
        public static void DocWebSite()
        {
            Application.OpenUrl(DemoApplication.Config.DocWebSite);
        }
    }
}