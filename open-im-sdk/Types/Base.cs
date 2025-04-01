using System;
using Newtonsoft.Json;

namespace OpenIM.IMSDK
{
    public class IMConfig
    {
        [JsonProperty("platformID")]
        public int PlatformID;
        [JsonProperty("apiAddr")]
        public string ApiAddr;
        [JsonProperty("wsAddr")]
        public string WsAddr;
        [JsonProperty("dataDir")]
        public string DataDir;
        [JsonProperty("logLevel")]
        public uint LogLevel;
        [JsonProperty("isLogStandardOutput")]
        public bool IsLogStandardOutput;
        [JsonProperty("logFilePath")]
        public string LogFilePath;
        [JsonProperty("isExternalExtensions")]
        public bool IsExternalExtensions;
    }

    class Empty
    {
    }
    class BoolValue
    {
        public bool value = false;
    }
    class StringValue
    {
        public string value = "";
    }
    class IntValue
    {
        public int value = 0;
    }
}