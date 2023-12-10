using EventsSO;
using Plugins.Event_System_SO.Scripts;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityMQTT.Events;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace UnityMQTT
{
    public class MqttClientSensor : MqttClientComponent
    {
        [Header("Topics")]
        [SerializeField] private string[] topics;

        [Header("UI Components")]
        [SerializeField]
        private Slider positionSlider;

        [Header("3D Model")]
        [SerializeField] private Transform barTransform;
        [SerializeField] private float zMaxLimit;
        [SerializeField] private float zMinLimit;
        [SerializeField] private int maxCounts;
        [SerializeField] private int minCounts;

        private Vector3 newBarPosition;

        [Header("Events SO")]
        [SerializeField] private VoidEventSO disconnectEvtSO;
        [SerializeField] private BrokerSettingsEvtSO connectEvtSO;
        [SerializeField] private MqttStatusEvtSO mqttStatusEvtSO;

        // MQTT.
        private string _message;
        private MqttData _data;

        #region Unity Methods

        protected void OnEnable()
        {
            // Suscribirse al evento de cambio de valor en el Slider.
            positionSlider.onValueChanged.AddListener(OnSliderValueChanged);

            // Suscribirse al evento de desconexión.
            disconnectEvtSO.AddObserver(Disconnect);

            // Suscribirse al evento de conexión.
            connectEvtSO.AddObserver(ConnectToBroker);
        }

        protected override void OnDisable()
        {
            // Desuscribirse del slider.
            positionSlider.onValueChanged.RemoveListener(OnSliderValueChanged);

            // Desuscribirse del evento de desconexión.
            disconnectEvtSO.RemoveObserver(Disconnect);

            // Desuscribirse del evento de conexión.
            connectEvtSO.RemoveObserver(ConnectToBroker);

            base.OnDisable();
        }

        protected override void Update()
        {
            ProcessMqttEvents();
        }

        #endregion Unity Methods

        #region MQTT topics methods

        protected override void SubscribeTopics()
        {
            client.Subscribe(topics, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(topics);
        }

        #endregion MQTT topics methods

        #region MQTT message processing

        protected override void DecodeMessage(string topic, byte[] message)
        {
            try
            {
                // Obtenemos los datos desde el archivo json.
                _data = JsonUtility.FromJson<MqttData>(Encoding.UTF8.GetString(message));

                // Movemos la barra según los counts.
                MoveBar(_data.Counts);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"Ocurrió un error al recibir los datos: {e}");
#endif
            }
        }

        #endregion MQTT message processing

        #region MQTT Events Callbacks

        private void ConnectToBroker(MqttBrokerSettingsSO settingsSO)
        {
            brokerAddress = settingsSO.IpAddress;
            brokerPort = settingsSO.Port;

            Connect();
        }

        protected override void OnConnecting()
        {
            mqttStatusEvtSO.Invoke(
                MqttStatus.Connecting,
                "Conectando",
                "Conectando con el servidor.");

            base.OnConnected();
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            mqttStatusEvtSO.Invoke(
                MqttStatus.ConnectionFailed,
                "Falló la conexión",
                $"No fue posible conectarse con {brokerAddress}:{brokerPort}");

            base.OnConnectionFailed(errorMessage);
        }

        protected override void OnConnected()
        {
            mqttStatusEvtSO.Invoke(
                MqttStatus.Connected,
                "¡Conectado!",
                $"Se estableció una conexión exitosa con {brokerAddress}:{brokerPort}");

            base.OnConnected();
        }

        protected override void OnConnectionClose()
        {
            mqttStatusEvtSO.Invoke(
                MqttStatus.ConnectionClose,
                "Desconectado",
                $"Se cerró la conexión con {brokerAddress}:{brokerPort}");

            base.OnConnectionClose();
        }

        #endregion MQTT Events Callbacks

        #region UI Callbacks

        private void OnSliderValueChanged(float value)
        {
            // Client check.
            if (client != null && client.IsConnected) return;

            // Move bar.
            MoveBar(value);
        }

        #endregion UI Callbacks

        #region Movement

        private void MoveBar(float counts)
        {
            // Referencia de la posición actual.
            newBarPosition = barTransform.localPosition;

            // Modificamos la posición.
            if (counts < 0) counts = 0;
            float valorNormalizado = counts / maxCounts;

            //newBarPosition.z = valorNormalizado;
            newBarPosition.z = Map(counts, minCounts, maxCounts, zMinLimit, zMaxLimit);

            // Asignamos la nueva posición.
            barTransform.localPosition = newBarPosition;
        }

        public float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        #endregion Movement
    }
}