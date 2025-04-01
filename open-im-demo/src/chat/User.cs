using OpenIM.IMSDK;
using IMDemo.Core;

namespace IMDemo.Chat
{
    public class User
    {
        public string uid;
        public string token;
        public LoginStatus loginStatus = LoginStatus.Empty;
        public ConversationListener conversationListener;
        public FriendShipListener friendShipListener;
        public GroupListener groupListener;
        public MessageListener messageListener;
        public int totalUnreadCount;

        public User(string uid, string token)
        {
            this.uid = uid;
            this.token = token;
            conversationListener = new ConversationListener();
            friendShipListener = new FriendShipListener();
            groupListener = new GroupListener();
            messageListener = new MessageListener();

            IMSDK.GetInstance().SetConversationListener(conversationListener);
            IMSDK.GetInstance().SetFriendShipListener(friendShipListener);
            IMSDK.GetInstance().SetGroupListener(groupListener);
            IMSDK.GetInstance().SetBatchMsgListener(messageListener);
        }

        public void Login()
        {
            IMSDK.GetInstance().Login(uid, token, (bool suc, int errCode, string errMsg) =>
            {
                if (suc)
                {
                    OnLoginSuc();
                }
                else
                {
                    Debug.Log(errCode, errMsg);
                }
            });
        }
        public void Logout()
        {
            IMSDK.GetInstance().Logout((bool suc, int errCode, string errMsg) =>
            {
                if (suc)
                {
                    Debug.Log($"{uid} Logout suc");
                    ChatMgr.Instance.currentUser = null;
                    ChatMgr.Application.Title = "IMDemo";
                }
                else
                {
                    Debug.Log(errCode, errMsg);
                }
            });
        }
        void OnLoginSuc()
        {
            uid = IMSDK.GetInstance().GetLoginUserId();
            ChatMgr.Application.Title = "IMDemo-" + uid;
            loginStatus = IMSDK.GetInstance().GetLoginStatus();
            IMSDK.GetInstance().GetTotalUnreadMsgCount((count, err, errMsg) =>
            {
                if (err > 0)
                {
                    Debug.Error(errMsg);
                }
                else
                {
                    totalUnreadCount = count;
                }
            });
        }

        public static void TryLogin(string uid, string token)
        {
            if (ChatMgr.Instance.currentUser == null)
            {
                var user = new User(uid, token);
                user.Login();
                ChatMgr.Instance.currentUser = user;
            }
            else
            {
                if (ChatMgr.Instance.currentUser.uid != uid)
                {
                    Application.CreateNewInstance(uid, token);
                }
                else
                {
                    Debug.Log("Already Login");
                }
            }
        }
    }
}

