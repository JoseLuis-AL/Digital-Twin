using Plugins.Event_System_SO.Scripts;
using UnityEngine;

namespace UnityMQTT.Events
{
    [CreateAssetMenu(fileName = "MQTT Broker Settings EvtSO", menuName = "MQTT/Events/Broker Settings", order = 0)]
    public class MqttConnectionSettingsEvtSO : EventSO<MqttConnectionSettingsSO>
    {
    }
}