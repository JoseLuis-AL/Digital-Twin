using System;

namespace UnityMQTT.Connection
{
    [Serializable]
    public class MqttConnectionStatusData
    {
        public MqttConnectionStatus Status;
        public string Title;
        public string Message;
    }
}