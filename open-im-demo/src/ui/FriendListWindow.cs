using IMDemo.Chat;
using ImGuiNET;
using OpenIM.IMSDK;
using IMDemo.Core;
using IMDemo.Core.UI;

namespace IMDemo.UI
{
    public class FriendListWindow : ImGuiWindow
    {
        [MenuItem("Friend/Friend List")]
        public static void ShowFriendInfo()
        {
            var window = GetWindow<FriendListWindow>();
            window.rect.w = 600;
            window.rect.h = 400;
            window.Show("Friend List");
        }

        List<FriendInfo> friendList;

        public override void OnEnable()
        {
            ChatMgr.Instance.currentUser.friendShipListener.Event_OnFriendInfoChange += OnFriendInfoChange;
            RefreshFriendList();
        }
        public override void OnClose()
        {
            ChatMgr.Instance.currentUser.friendShipListener.Event_OnFriendInfoChange -= OnFriendInfoChange;
        }
        void RefreshFriendList()
        {
            IMSDK.GetInstance().GetFriendList((list, errCode, errMsg) =>
           {
               if (list != null)
               {
                   friendList = list;
               }
           }, true);
        }
        void OnFriendInfoChange(FriendInfo friend)
        {
            RefreshFriendList();
        }
        public override void OnGUI()
        {
            if (friendList == null) return;

            ImGui.BeginChild("TableContainer", new System.Numerics.Vector2(0, rect.h - 50), true, ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.AlwaysVerticalScrollbar);

            if (ImGui.BeginTable("FriendInfoTable", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollX))
            {
                ImGui.TableSetupColumn("Nickname");
                ImGui.TableSetupColumn("FaceURL");
                ImGui.TableSetupColumn("Remark");
                ImGui.TableSetupColumn("Chat");
                ImGui.TableSetupColumn("Delete");
                ImGui.TableSetupColumn("Detail");

                ImGui.TableHeadersRow();

                for (int i = 0; i < friendList.Count; i++)
                {
                    var friend = friendList[i];
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text(friend.Nickname);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(friend.FaceURL);

                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text(friend.Remark);

                    ImGui.TableSetColumnIndex(3);
                    ImGui.PushID("Btn_Chat" + i);
                    if (ImGui.Button("Chat"))
                    {
                        OnChatClick(friend);
                    }
                    ImGui.PopID();

                    ImGui.TableSetColumnIndex(4);
                    ImGui.PushID("Btn_Delete" + i);
                    if (ImGui.Button("Delete"))
                    {
                        OnDeleteClick(friend);
                    }
                    ImGui.PopID();

                    ImGui.TableSetColumnIndex(5);

                    ImGui.PushID("Btn_Detail" + i);
                    if (ImGui.Button("Detail"))
                    {
                        OnDetailClick(friend);
                    }
                    ImGui.PopID();
                }

                ImGui.EndTable();
            }

            ImGui.EndChild();
        }

        void OnChatClick(FriendInfo friend)
        {
            IMSDK.GetInstance().GetOneConversation((converstion, err, errMsg) =>
            {
                if (converstion != null)
                {
                    ChatWindow.ShowChatWindow(converstion);
                }
                else
                {
                    Debug.Error(errMsg);
                }
            }, ConversationType.Single, friend.FriendUserID);
        }
        void OnDeleteClick(FriendInfo friend)
        {
            IMSDK.GetInstance().DeleteFriend((suc, err, errMsg) =>
            {
                if (suc)
                {
                    RefreshFriendList();
                }
                else
                {
                    Debug.Error(errMsg);
                }
            }, friend.FriendUserID);
        }
        void OnDetailClick(FriendInfo friend)
        {
            FriendInfoWindow.ShowFriendInfo(friend.FriendUserID);
        }
    }
}