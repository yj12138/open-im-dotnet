
namespace IMDemo
{
    public class Config
    {
        public class Account
        {
            public string UserId;
            public string Token;
        }
        public string OfficialWebSite;
        public string DocWebSite;
        public string APIAddr;
        public string WsAddr;
        public string DataDir;
        public string DBPath;
        public uint LogLevel;
        public bool IsExternalExtensions;
        public bool IsLogStandardOutput;
        public string LogFilePath;
        public string AdminToken;
        public Account[] TestAccounts;
    }
}

