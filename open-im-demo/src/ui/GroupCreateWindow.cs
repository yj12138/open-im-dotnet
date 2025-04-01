using IMDemo.Chat;
using ImGuiNET;
using OpenIM.IMSDK;
using IMDemo.Core;
using IMDemo.Core.UI;

namespace IMDemo.UI
{
    public class GroupCreateWindow : ImGuiWindow
    {
        [MenuItem("Group/Create Group")]
        public static void ShowCreateGroupWindow()
        {
            var window = GetWindow<GroupCreateWindow>();
            window.rect.w = 600;
            window.rect.h = 400;
            window.Show("Group Create");
        }
        CreateGroupReq createGroupReq;
        List<FriendInfo> friendList;
        List<bool> isSelectList;
        public override void OnEnable()
        {

            createGroupReq = new CreateGroupReq()
            {
                OwnerUserID = ChatMgr.Instance.currentUser.uid,
                GroupInfo = new GroupInfo()
                {
                    GroupName = "",
                    FaceURL = "",
                    GroupType = (int)GroupType.Group,
                }
            };
            isSelectList = new List<bool>();
            IMSDK.GetInstance().GetFriendList((list, errCode, errMsg) =>
           {
               if (list != null)
               {
                   friendList = list;
                   for (int i = 0; i < friendList.Count; i++)
                   {
                       isSelectList.Add(false);
                   }
               }
           }, true);
        }
        public override void OnGUI()
        {
            ImGui.Columns(2, "CreateGroupInfoColumns", false);

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupName");
            ImGui.NextColumn();
            ImGui.InputText("##GroupName", ref createGroupReq.GroupInfo.GroupName, 100);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupFace");
            ImGui.NextColumn();
            ImGui.InputText("##GroupFace", ref createGroupReq.GroupInfo.FaceURL, 100);
            ImGui.NextColumn();

            ImGui.Columns(1);

            ImGui.Text("Select Member:");
            if (friendList != null)
            {
                ImGui.BeginChild("TableContainer", new System.Numerics.Vector2(0, 200), true, ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.AlwaysVerticalScrollbar);
                if (ImGui.BeginTable("FriendInfoTable", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY))
                {
                    ImGui.TableSetupColumn("NickName");
                    ImGui.TableSetupColumn("Select");

                    ImGui.TableHeadersRow();
                    for (int i = 0; i < friendList.Count; i++)
                    {
                        ImGui.TableNextRow();

                        ImGui.TableSetColumnIndex(0);
                        ImGui.Text(friendList[i].Nickname);

                        ImGui.TableSetColumnIndex(1);
                        var isSelect = isSelectList[i];
                        if (ImGui.Checkbox("##isSelect", ref isSelect))
                        {
                            isSelectList[i] = isSelect;
                        }
                    }

                    ImGui.EndTable();
                }
                ImGui.EndChild();
            }
            if (ImGui.Button("Create Group"))
            {
                var memeberUserIds = new List<string>();
                for (int i = 0; i < friendList.Count; i++)
                {
                    if (isSelectList[i])
                    {
                        memeberUserIds.Add(friendList[i].FriendUserID);
                    }
                }
                createGroupReq.MemberUserIDs = memeberUserIds.ToArray();
                IMSDK.GetInstance().CreateGroup((group, err, errMsg) =>
                {
                    if (group != null)
                    {
                        Close();
                    }
                    else
                    {
                        Debug.Error(errMsg);
                    }
                }, createGroupReq);
            }
        }
    }
}