using OpenIM.IMSDK;
using OpenIM.IMSDK.Listener;

namespace IMDemo.Chat
{
    public class GroupListener : IGroupListener
    {

        public event Action<GroupInfo> Event_OnGroupAdd;
        public event Action<GroupInfo> Event_OnGroupDel;
        public event Action<GroupInfo> Event_OnGroupInfoChange;
        public GroupListener()
        {
        }

        public void OnGroupApplicationAccepted(GroupApplicationInfo groupApplication)
        {
        }

        public void OnGroupApplicationAdded(GroupApplicationInfo groupApplication)
        {
        }

        public void OnGroupApplicationDeleted(GroupApplicationInfo groupApplication)
        {
        }

        public void OnGroupApplicationRejected(GroupApplicationInfo groupApplication)
        {
        }

        public void OnGroupDismissed(GroupInfo groupInfo)
        {
        }

        public void OnGroupInfoChanged(GroupInfo groupInfo)
        {
            Event_OnGroupInfoChange?.Invoke(groupInfo);
        }

        public void OnGroupMemberAdded(GroupMember groupMemberInfo)
        {
        }

        public void OnGroupMemberDeleted(GroupMember groupMemberInfo)
        {
        }

        public void OnGroupMemberInfoChanged(GroupMember groupMemberInfo)
        {
        }

        public void OnJoinedGroupAdded(GroupInfo groupInfo)
        {
            Event_OnGroupAdd?.Invoke(groupInfo);
        }

        public void OnJoinedGroupDeleted(GroupInfo groupInfo)
        {
            Event_OnGroupDel?.Invoke(groupInfo);
        }
    }
}

