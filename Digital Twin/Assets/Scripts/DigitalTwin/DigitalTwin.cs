using Plugins.Event_System_SO.Scripts;
using System;
using System.Text;
using UnityEngine;
using UnityMQTT;
using UnityMQTT.Connection;
using UnityMQTT.Events;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DigitalTwin
{
    public class DigitalTwin : MqttClientComponent
    {
        [SerializeField] private string _topic;

        [Header("3D Model")]
        [SerializeField] private Transform _barTransform;
        [SerializeField] private float _zMaxLimit;
        [SerializeField] private float _zMinLimit;
        [SerializeField] private float _maxCounts;
        [SerializeField] private float _minCounts;
        private Vector3 _newBarPosition;

        [Header("Connection Status")]
        [SerializeField] private MqttConnectionStatusData _onConnecting;
        [SerializeField] private MqttConnectionStatusData _onConnected;
        [SerializeField] private MqttConnectionStatusData _onConnectionFailed;
        [SerializeField] private MqttConnectionStatusData _onConnectionClose;

        [Header("Events SO")]
        [SerializeField] private VoidEventSO _disconnectEvtSO;
        [SerializeField] private MqttConnectionSettingsEvtSO _connectEvtSO;
        [SerializeField] private MqttConnectionStatusEvtSO _connectionStatusEvtSO;

        // MQTT data.
        private Data _data;
        private string _message;

        #region Unity

        private void OnEnable()
        {
            _connectEvtSO.AddObserver(ConnectToBroker);
            _disconnectEvtSO.AddObserver(Disconnect);
        }

        protected override void OnDisable()
        {
            _connectEvtSO.RemoveObserver(ConnectToBroker);
            _disconnectEvtSO.RemoveObserver(Disconnect);

            base.OnDisable();
        }

        #endregion Unity

        #region MQTT Topics

        protected override void SubscribeTopics()
        {
            client.Subscribe(new[] { _topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new[] { _topic });
        }

        #endregion MQTT Topics

        #region MQTT Events Callbacks

        private void ConnectToBroker(MqttConnectionSettingsSO settings)
        {
            // Set the broker settings.
            brokerAddress = settings.IpAddress;
            brokerPort = settings.Port;
            _topic = settings.Topic;

            // Connect to the broker.
            Connect();
        }

        protected override void OnConnecting()
        {
            _connectionStatusEvtSO.Invoke(_onConnecting);
            base.OnConnecting();
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            _connectionStatusEvtSO.Invoke(_onConnectionFailed);
            base.OnConnectionFailed(errorMessage);
        }

        protected override void OnConnected()
        {
            _connectionStatusEvtSO.Invoke(_onConnected);
            base.OnConnected();
        }

        protected override void OnConnectionClose()
        {
            _connectionStatusEvtSO.Invoke(_onConnectionClose);
            base.OnConnectionClose();
        }

        #endregion MQTT Events Callbacks

        #region MQTT Message Processing

        protected override void DecodeMessage(string topic, byte[] message)
        {
            try
            {
                // Convert the file from JSON format.
                _data = JsonUtility.FromJson<Data>(Encoding.UTF8.GetString(message));

                // Calculate the new position of the bar using the counts.
                MoveBar(_data.Counts);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"Format error when converting data.: {e}");
#endif
            }
        }

        #endregion MQTT Message Processing

        #region Model Movement

        private void MoveBar(float counts)
        {
            // Avoid impossible values in the model.
            if (counts < _minCounts) counts = _minCounts;
            else if (counts > _maxCounts) counts = _maxCounts;

            // Get the current position of the bar.
            _newBarPosition = _barTransform.localPosition;

            // Calculate the new position in z with respect to the number of revolutions.
            _newBarPosition.z = Map(counts, _minCounts, _maxCounts, _zMinLimit, _zMaxLimit);

            // Assign the new position in z.
            _barTransform.localPosition = _newBarPosition;
        }

        private float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        #endregion Model Movement
    }
}