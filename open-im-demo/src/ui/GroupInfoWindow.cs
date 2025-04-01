using ImGuiNET;
using OpenIM.IMSDK;
using IMDemo.Core;
using IMDemo.Core.UI;

namespace IMDemo.UI
{
    public class GroupInfoWindow : ImGuiWindow
    {
        public static void ShowGroupInfo(string groupId)
        {
            var window = GetWindow<GroupInfoWindow>();
            window.groupId = groupId;
            window.rect.w = 600;
            window.rect.h = 400;
            window.Show("Group Info");
        }
        public string groupId;
        public GroupInfo group;

        public override void OnEnable()
        {
            IMSDK.GetInstance().GetSpecifiedGroupsInfo((groupList, err, errMsg) =>
            {
                if (groupList != null)
                {
                    if (groupList.Count == 1)
                    {
                        group = groupList[0];
                    }
                }
                else
                {
                    Debug.Error(errMsg);
                }
            }, [groupId]);
        }

        public override void OnGUI()
        {
            if (group == null) return;
            ImGui.Columns(2, "GroupInfoColumns", false);

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupID");
            ImGui.NextColumn();
            ImGui.Text(group.GroupID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupName");
            ImGui.NextColumn();
            if (ImGui.InputText("##groupName", ref group.GroupName, 100)) { }
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("Notification");
            ImGui.NextColumn();
            if (ImGui.InputText("##notification", ref group.Notification, 100)) { }
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("Introduction");
            ImGui.NextColumn();
            if (ImGui.InputText("##introduction", ref group.Introduction, 100)) { }
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("FaceURL");
            ImGui.NextColumn();
            if (ImGui.InputText("##faceURL", ref group.FaceURL, 100)) { }
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("CreateTime");
            ImGui.NextColumn();
            ImGui.Text($"{Time.GetTimeStampStr(group.CreateTime / 1000)}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("Status");
            ImGui.NextColumn();
            ImGui.Text($"{group.Status}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("CreatorUserID");
            ImGui.NextColumn();
            ImGui.Text(group.CreatorUserID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupType");
            ImGui.NextColumn();
            ImGui.Text($"{group.GroupType}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("OwnerUserID");
            ImGui.NextColumn();
            ImGui.Text(group.OwnerUserID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("MemberCount");
            ImGui.NextColumn();
            ImGui.Text($"{group.MemberCount}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("Ex");
            ImGui.NextColumn();
            ImGui.InputText("##Ex", ref group.Ex, 1000);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("AttachedInfo");
            ImGui.NextColumn();
            ImGui.Text(group.AttachedInfo);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("NeedVerification");
            ImGui.NextColumn();
            ImGui.InputInt("##NeedVerification", ref group.NeedVerification);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("LookMemberInfo");
            ImGui.NextColumn();
            ImGui.Text($"{group.LookMemberInfo}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("ApplyMemberFriend");
            ImGui.NextColumn();
            ImGui.Text($"{group.ApplyMemberFriend}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("NotificationUpdateTime");
            ImGui.NextColumn();
            ImGui.Text($"{group.NotificationUpdateTime}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("NotificationUserID");
            ImGui.NextColumn();
            ImGui.Text(group.NotificationUserID);
            ImGui.NextColumn();

            ImGui.Columns(1);

            if (ImGui.Button("Modify"))
            {
                IMSDK.GetInstance().SetGroupInfo((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        Close();
                    }
                    else
                    {
                        Debug.Error(errMsg);
                    }
                }, new GroupInfoForSet()
                {
                    GroupID = group.GroupID,
                    GroupName = group.GroupName,
                    Notification = group.Notification,
                    Introduction = group.Introduction,
                    FaceURL = group.FaceURL,
                    Ex = group.Ex,
                    NeedVerification = group.NeedVerification,
                });
            }
        }
    }
}