using Plugins.Event_System_SO.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityMQTT;
using UnityMQTT.Connection;
using UnityMQTT.Events;

namespace UI.ModalWindow
{
    public class ConnectionModalWindow : ModalWindow
    {
        [Header("Window")]
        [SerializeField] private bool _hideOnStart;

        [Header("Broker Data")]
        [SerializeField] private MqttConnectionSettingsSO _settingsSO;

        [Header("Broker Settings")]
        [SerializeField] private TMP_InputField _brokerAddressInputField;
        [SerializeField] private TMP_InputField _topicInputField;
        [SerializeField] private TMP_InputField _portInputField;

        [Header("Buttons")]
        [SerializeField] private Button _connectBtn;
        [SerializeField] private Button _disconnectBtn;
        [SerializeField] private Button _cancelBtn;

        [Header("Events SO")]
        [SerializeField] private MqttConnectionSettingsEvtSO _connectEvtSO;
        [SerializeField] private MqttConnectionStatusEvtSO _connectionStatusEvtSO;
        [SerializeField] private VoidEventSO _disconnectEvtSO;

        #region Unity

        private void Awake()
        {
            if (_hideOnStart) HideWindow();
        }

        private void OnEnable()
        {
            _connectionStatusEvtSO.AddObserver(OnStatusChanged);

            _connectBtn.onClick.AddListener(Connect);
            _disconnectBtn.onClick.AddListener(Disconnect);
            _cancelBtn.onClick.AddListener(HideWindow);
        }

        private void OnDisable()
        {
            _connectionStatusEvtSO.RemoveObserver(OnStatusChanged);

            _connectBtn.onClick.RemoveListener(Connect);
            _disconnectBtn.onClick.RemoveListener(Disconnect);
            _cancelBtn.onClick.RemoveListener(HideWindow);
        }

        #endregion Unity

        #region MQTT Connection

        private void Connect()
        {
            // Get the values from the UI.
            var brokerAddress = _brokerAddressInputField.text;
            var topic = _topicInputField.text;
            var brokerPort = _portInputField.text;

            // Check the data.
            if (string.IsNullOrEmpty(brokerAddress) || string.IsNullOrWhiteSpace(brokerAddress) ||
                string.IsNullOrEmpty(topic) || string.IsNullOrWhiteSpace(topic) ||
                string.IsNullOrEmpty(brokerPort) || string.IsNullOrWhiteSpace(brokerPort)) return;

            // Save the data to the OS and invoke the event.
            _settingsSO.IpAddress = brokerAddress;
            _settingsSO.Topic = topic;
            _settingsSO.Port = int.Parse(brokerPort);
            _connectEvtSO.Invoke(_settingsSO);

            // Hide the window.
            HideWindow();
        }

        private void Disconnect()
        {
            _disconnectEvtSO.Invoke();
            HideWindow();
        }

        private void OnStatusChanged(MqttConnectionStatusData newStatusData)
        {
            // Check the connection.
            if (newStatusData.Status == MqttConnectionStatus.Connected || newStatusData.Status == MqttConnectionStatus.Connecting)
            {
                _connectBtn.gameObject.SetActive(false);
                _disconnectBtn.gameObject.SetActive(true);
            }
            else if (newStatusData.Status == MqttConnectionStatus.ConnectionClose || newStatusData.Status == MqttConnectionStatus.ConnectionFailed)
            {
                _connectBtn.gameObject.SetActive(true);
                _disconnectBtn.gameObject.SetActive(false);
            }
        }

        #endregion MQTT Connection
    }
}