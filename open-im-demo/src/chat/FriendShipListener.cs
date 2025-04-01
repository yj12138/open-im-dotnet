using OpenIM.IMSDK;
using OpenIM.IMSDK.Listener;

namespace IMDemo.Chat
{
    public class FriendShipListener : IFriendShipListener
    {

        public event Action<FriendInfo> Event_OnFriendInfoChange;
        public FriendShipListener()
        {
        }


        public void OnBlackAdded(BlackInfo blackInfo)
        {
        }

        public void OnBlackDeleted(BlackInfo blackInfo)
        {
        }

        public void OnFriendAdded(FriendInfo friendInfo)
        {
        }

        public void OnFriendDeleted(FriendInfo friendInfo)
        {

        }

        public void OnFriendInfoChanged(FriendInfo friendInfo)
        {
            Event_OnFriendInfoChange?.Invoke(friendInfo);
        }

        public void OnFriendApplicationAdded(FriendApplicationInfo friendApplication)
        {
        }

        public void OnFriendApplicationDeleted(FriendApplicationInfo friendApplication)
        {
        }

        public void OnFriendApplicationAccepted(FriendApplicationInfo friendApplication)
        {
        }

        public void OnFriendApplicationRejected(FriendApplicationInfo friendApplication)
        {
        }
    }
}
