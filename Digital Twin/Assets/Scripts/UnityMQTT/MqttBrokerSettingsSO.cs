using UnityEngine;

namespace UnityMQTT
{
    [CreateAssetMenu(fileName = "Broker Settings SO", menuName = "MQTT/Broker Settings SO", order = 0)]
    public class MqttBrokerSettingsSO : ScriptableObject
    {
        [SerializeField] private string ipAddress;
        [SerializeField] private string topic;
        [SerializeField] private int port;

        public string IpAddress
        {
            get => ipAddress;
            set => ipAddress = value;
        }

        public string Topic
        {
            get => topic;
            set => topic = value;
        }

        public int Port
        {
            get => port;
            set => port = value;
        }
    }
}