using ImGuiNET;
using OpenIM.IMSDK;
using IMDemo.Core;
using IMDemo.Core.UI;

namespace IMDemo.UI
{
    public class ConversationInfoWindow : ImGuiWindow
    {
        public static void ShowConversationInfo(string conversationId)
        {
            var window = GetWindow<ConversationInfoWindow>();
            window.conversationId = conversationId;
            window.rect.w = 600;
            window.rect.h = 400;
            window.Show("Conversation");
        }
        public string conversationId;
        public Conversation conversation;
        public override void OnEnable()
        {
            conversation = null;
            IMSDK.GetInstance().GetMultipleConversation((list, err, errMsg) =>
            {
                if (list != null)
                {
                    if (list.Count == 1)
                    {
                        conversation = list[0];
                    }
                }
                else
                {
                    Debug.Error(errMsg);
                }
            }, [conversationId]);
        }


        public override void OnClose()
        {
            base.OnClose();
            conversation = null;

        }

        public override void OnGUI()
        {
            if (conversation == null) return;
            ImGui.Columns(2, "ConversationInfoColumns", false);

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("ConversationID");
            ImGui.NextColumn();
            ImGui.Text(conversation.ConversationID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("ConversationType");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.ConversationType}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("UserID");
            ImGui.NextColumn();
            ImGui.Text(conversation.UserID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupID");
            ImGui.NextColumn();
            ImGui.Text(conversation.GroupID);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("ShowName");
            ImGui.NextColumn();
            ImGui.Text(conversation.ShowName);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("FaceURL");
            ImGui.NextColumn();
            ImGui.Text(conversation.FaceURL);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("RecvMsgOpt");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.RecvMsgOpt}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("UnreadCount");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.UnreadCount}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("GroupAtType");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.GroupAtType}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("LatestMsg");
            ImGui.NextColumn();
            ImGui.Text(conversation.LatestMsg);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("LatestMsgSendTime");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.LatestMsgSendTime}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("DraftText");
            ImGui.NextColumn();
            if (ImGui.InputText("##draftText", ref conversation.DraftText, 500)) { }
            ImGui.SameLine();
            if (ImGui.Button("SetDraftText"))
            {
                IMSDK.GetInstance().SetConversationDraft((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        IMSDK.GetInstance().GetOneConversation((newConversation, err, errMsg) =>
                       {
                           if (newConversation != null)
                           {
                               conversation = newConversation;
                           }
                       }, conversation.ConversationType, conversation.ConversationType == ConversationType.Single ? conversation.UserID : conversation.GroupID);
                    }
                }, conversationId, conversation.DraftText);
            }
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("DraftTextTime");
            ImGui.NextColumn();
            ImGui.Text($"{Time.GetTimeStampStr(conversation.DraftTextTime / 1000)}");

            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("IsPinned");
            ImGui.NextColumn();
            ImGui.Checkbox("##IsPinned", ref conversation.IsPinned);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("IsPrivateChat");
            ImGui.NextColumn();
            ImGui.Checkbox("##IsPrivateChat", ref conversation.IsPrivateChat);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("BurnDuration");
            ImGui.NextColumn();
            ImGui.InputInt("##BurnDuration", ref conversation.BurnDuration);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("IsNotInGroup");
            ImGui.NextColumn();
            ImGui.Text(conversation.IsNotInGroup.ToString());
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("UpdateUnreadCountTime");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.UpdateUnreadCountTime}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("AttachedInfo");
            ImGui.NextColumn();
            ImGui.Text(conversation.AttachedInfo);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("Ex");
            ImGui.NextColumn();
            ImGui.InputText("##Ex", ref conversation.Ex, 500);
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("MaxSeq");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.MaxSeq}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("MinSeq");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.MinSeq}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("HasReadSeq");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.HasReadSeq}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("MsgDestructTime");
            ImGui.NextColumn();
            ImGui.Text($"{conversation.MsgDestructTime}");
            ImGui.NextColumn();

            ImGui.SetColumnWidth(0, 150);
            ImGui.Text("IsMsgDestruct");
            ImGui.NextColumn();
            ImGui.Text(conversation.IsMsgDestruct.ToString());
            ImGui.NextColumn();

            ImGui.Columns(1);

            if (ImGui.Button("Modify"))
            {
                IMSDK.GetInstance().SetConversation((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        Close();
                    }
                    else
                    {
                        Debug.Error(errMsg);
                    }
                }, conversation.ConversationID, new ConversationReq()
                {
                    IsPinned = conversation.IsPinned,
                    IsPrivateChat = conversation.IsPrivateChat,
                    Ex = conversation.Ex,
                    BurnDuration = conversation.BurnDuration,
                });
            }
        }
    }
}