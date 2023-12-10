using Plugins.Event_System_SO.Scripts;
using UnityEngine;

namespace UnityMQTT.Events
{
    [CreateAssetMenu(fileName = "MQTT Status Event SO", menuName = "MQTT/MQTT Status Event SO", order = 0)]
    public class MqttStatusEvtSO : EventSO<MqttStatus, string, string>
    {
       
    }
}
