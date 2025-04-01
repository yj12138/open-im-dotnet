using OpenIM.IMSDK;
using OpenIM.IMSDK.Listener;

namespace IMDemo.Chat
{
    public class ConversationListener : IConversationListener
    {

        public event Action<List<Conversation>> Event_OnConversationsInfoChange;

        public ConversationListener()
        {
        }

        public void OnConversationChanged(List<Conversation> conversationList)
        {
            Event_OnConversationsInfoChange?.Invoke(conversationList);
        }

        public void OnNewConversation(List<Conversation> conversationList)
        {
        }

        public void OnSyncServerFailed()
        {
        }

        public void OnSyncServerStart()
        {
        }

        public void OnTotalUnreadMessageCountChanged(int totalUnreadCount)
        {
        }

        public void OnConversationUserInputStatusChanged(InputStatesChangedData data)
        {
        }

        public void OnSyncServerProgress(int progress)
        {
        }

        public void OnSyncServerFinish()
        {
        }
    }
}
