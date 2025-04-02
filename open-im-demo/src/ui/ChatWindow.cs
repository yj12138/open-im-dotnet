using System.Numerics;
using IMDemo.Chat;
using ImGuiNET;
using OpenIM.IMSDK;
using IMDemo.Core.UI;
using IMDemo.Core;

namespace IMDemo.UI
{
    public class ChatWindow : ImGuiWindow
    {
        class SendMsgCallBack : IMsgSendCallBack
        {
            public Action<int, string> OnErrorCB;
            public Action<long> OnProgressCB;
            public Action<Message> OnSuccessCB;

            public void OnError(int code, string errMsg)
            {
                if (OnErrorCB != null)
                {
                    OnErrorCB(code, errMsg);
                }
            }

            public void OnProgress(long progress)
            {
                if (OnProgressCB != null)
                {
                    OnProgressCB(progress);
                }
            }

            public void OnSuccess(Message msg)
            {
                if (OnSuccessCB != null)
                {
                    OnSuccessCB(msg);
                }
            }
        }

        public static void ShowChatWindow(Conversation conversation)
        {
            var window = GetWindow<ChatWindow>();
            window.conversation = conversation;
            window.rect.w = 600;
            window.rect.h = 500;
            window.Show(conversation.ShowName);
        }

        public Conversation conversation;
        public List<Message> historyMessage;
        public string inputTextMessage;
        public string inputFilePath;
        public string inputSoundPath;
        public override void OnEnable()
        {
            inputTextMessage = "";
            inputFilePath = "";
            inputSoundPath = "";
            IMSDK.GetInstance().GetAdvancedHistoryMessageList((res, err, errMsg) =>
            {
                if (res != null)
                {
                    historyMessage = [.. res.MessageList];
                }
            }, new GetAdvancedHistoryMessageListParams
            {
                ConversationID = conversation.ConversationID,
                LastMinSeq = 0,
                StartClientMsgID = "",
                Count = 100
            });
            var user = ChatMgr.Instance.currentUser;
            if (user != null)
            {
                user.messageListener.Event_OnRecvNewMessages += OnRecvMessage;
            }
        }

        public override void OnGUI()
        {
            if (conversation == null) return;

            DrawMessageList();

            DrawTextInput();
            DrawFileInput();
            DrawSoundInput();
        }

        public override void OnClose()
        {
            base.OnClose();
            var user = ChatMgr.Instance.currentUser;
            if (user != null)
            {
                user.messageListener.Event_OnRecvNewMessages -= OnRecvMessage;
            }
        }
        bool scrollToBottom = true;
        void DrawMessageList()
        {
            ImGui.BeginChild("ChatMessages", new Vector2(ImGui.GetWindowWidth() - 50, ImGui.GetWindowHeight() - 200), true, ImGuiWindowFlags.AlwaysVerticalScrollbar);

            if (historyMessage != null)
            {
                var senderNickName = "";
                foreach (var message in historyMessage)
                {
                    if (senderNickName != message.SenderNickname)
                    {
                        ImGui.Text(message.SenderNickname + ":");
                    }
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 50);
                    if (message.TextElem != null)
                    {
                        ImGui.Text(message.TextElem.Content);
                    }
                    if (message.FileElem != null)
                    {
                        ImGui.Text("Recv File : URL=" + message.FileElem.SourceURL);
                    }
                    if (message.NotificationElem != null)
                    {
                        var notification = Notification.Parse(message.ContentType, message.NotificationElem.Detail);
                        if (notification is FriendApplicationApprovedTips)
                        {
                            var tip = notification as FriendApplicationApprovedTips;
                            ImGui.Text("FriendApplicationApprovedTips:" + tip.FromToUserID.FromUserID + tip.FromToUserID.ToUserID);
                        }
                    }
                    senderNickName = message.SenderNickname;
                }
                if (scrollToBottom)
                {
                    ImGui.SetScrollHereY(1.0f);
                    scrollToBottom = false;
                }
            }

            ImGui.EndChild();
        }

        void DrawTextInput()
        {
            ImGui.BeginGroup();
            {
                ImGui.Text("TextInput:");
                ImGui.InputText("##InputText", ref inputTextMessage, 1000);
                ImGui.SameLine();
                ImGui.PushID("Btn_SendText");
                if (ImGui.Button("Send"))
                {
                    Debug.Log("Send Text");
                    if (!string.IsNullOrEmpty(inputTextMessage))
                    {
                        var msg = IMSDK.GetInstance().CreateTextMessage(inputTextMessage);
                        SendMessage(msg);
                        inputTextMessage = "";
                    }
                }
                ImGui.PopID();
            }
            ImGui.EndGroup();
        }

        void DrawFileInput()
        {
            ImGui.BeginGroup();
            {
                ImGui.Text("File Path:");
                ImGui.InputText("##InputFilePath:", ref inputFilePath, 1000);
                ImGui.SameLine();
                ImGui.PushID("Btn_SendFile");
                if (ImGui.Button("Send"))
                {
                    if (!string.IsNullOrEmpty(inputFilePath) && File.Exists(inputFilePath))
                    {
                        var fileName = Path.GetFileNameWithoutExtension(inputFilePath);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var msg = IMSDK.GetInstance().CreateFileMessageFromFullPath(inputFilePath, fileName);
                            SendMessage(msg);
                            inputFilePath = "";
                        }
                    }
                }
                ImGui.PopID();
            }
            ImGui.EndGroup();
        }
        void DrawSoundInput()
        {
            ImGui.BeginGroup();
            {
                ImGui.Text("Sound File Path:");
                ImGui.InputText("##Sound Path:", ref inputSoundPath, 1000);
                ImGui.SameLine();
                ImGui.PushID("Btn_SendSound");
                if (ImGui.Button("Send"))
                {
                    Debug.Log("Send Sound");
                    if (!string.IsNullOrEmpty(inputSoundPath))
                    {
                        var msg = IMSDK.GetInstance().CreateSoundMessageFromFullPath(inputSoundPath, 1);
                        SendMessage(msg);
                        inputSoundPath = "";
                    }
                }
                ImGui.PopID();
            }
            ImGui.EndGroup();

        }

        public void SendMessage(Message msg)
        {
            var msgCallBack = new SendMsgCallBack();
            msgCallBack.OnSuccessCB = (_msg) =>
            {
                historyMessage.Add(_msg);
                scrollToBottom = true;
                Debug.Log("Send Suc");
            };
            msgCallBack.OnProgressCB = (progress) =>
            {
                Debug.Log("Send Progress:", progress);
            };
            msgCallBack.OnErrorCB = (errCode, errMsg) =>
            {
                Debug.Error(errMsg);
            };
            IMSDK.GetInstance().SendMessage(msgCallBack, msg, conversation.UserID, conversation.GroupID, new OfflinePushInfo(), false);
        }

        public void OnRecvMessage(List<Message> msgLists)
        {
            foreach (var msg in msgLists)
            {
                if (msg.SessionType == ConversationType.Single)
                {
                    if (msg.SendID == conversation.UserID)
                    {
                        historyMessage.Add(msg);
                    }
                }
                else if (msg.SessionType == ConversationType.Group)
                {
                    if (msg.RecvID == conversation.GroupID)
                    {
                        historyMessage.Add(msg);
                    }
                }
                scrollToBottom = true;
            }
        }
    }
}