using UnityEngine;

namespace UnityMQTT
{
    [CreateAssetMenu(fileName = "MQTT Connection Settings SO", menuName = "MQTT/Connection Settings SO", order = 0)]
    public class MqttConnectionSettingsSO : ScriptableObject
    {
        /// <summary>
        /// IP address of the broker.
        /// </summary>
        [Header("MQTT Connection Settings")]
        [SerializeField] private string ipAddress;

        /// <summary>
        /// Topic to which the client subscribes.
        /// </summary>
        [SerializeField] private string topic;

        /// <summary>
        /// Port through which the connection to the broker is established.
        /// </summary>
        [SerializeField] private int port;

        #region Properties

        /// <summary>
        /// IP address of the broker.
        /// </summary>
        public string IpAddress
        {
            get => ipAddress;
            set => ipAddress = value;
        }

        /// <summary>
        /// Topic to which the client subscribes.
        /// </summary>
        public string Topic
        {
            get => topic;
            set => topic = value;
        }

        /// <summary>
        /// Port through which the connection to the broker is established.
        /// </summary>
        public int Port
        {
            get => port;
            set => port = value;
        }

        #endregion Properties
    }
}