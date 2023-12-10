using Plugins.Event_System_SO.Scripts;
using UnityEngine;
using UnityMQTT.Connection;

namespace UnityMQTT.Events
{
    [CreateAssetMenu(fileName = "MQTT Connection Status EvtSO", menuName = "MQTT/Events/Connection Status", order = 0)]
    public class MqttConnectionStatusEvtSO : EventSO<MqttConnectionStatusData>
    {
    }
}