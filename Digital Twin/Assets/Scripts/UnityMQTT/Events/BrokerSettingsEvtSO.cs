using Plugins.Event_System_SO.Scripts;
using UnityEngine;
using UnityMQTT;

namespace EventsSO
{
    [CreateAssetMenu(fileName = "Broker Settings SO", menuName = "MQTT/Broker Settings Event SO", order = 0)]
    public class BrokerSettingsEvtSO : EventSO<MqttBrokerSettingsSO>
    {
    }
}
