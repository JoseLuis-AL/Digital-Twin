using UnityEngine;

namespace UnityMQTT.Events
{
    public enum MqttStatus
    {
        Connecting,
        Connected,
        ConnectionFailed,
        ConnectionClose
    }
}
