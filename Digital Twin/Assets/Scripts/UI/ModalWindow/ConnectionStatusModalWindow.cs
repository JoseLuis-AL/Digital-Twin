using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityMQTT;
using UnityMQTT.Connection;
using UnityMQTT.Events;

namespace UI.ModalWindow
{
    public class ConnectionStatusModalWindow : ModalWindow
    {
        [Header("Window")]
        [SerializeField] private bool _hideOnStart;

        [Header("UI Components")]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Button _acceptBtn;

        [Header("Events SO")]
        [SerializeField] private MqttConnectionStatusEvtSO _connectionStatusEvtSO;

        #region Unity

        private void Awake()
        {
            if (_hideOnStart) HideWindow();
        }

        private void OnEnable()
        {
            _connectionStatusEvtSO.AddObserver(OnStatusChanged);
            _acceptBtn.onClick.AddListener(HideWindow);
        }

        private void OnDisable()
        {
            _connectionStatusEvtSO.AddObserver(OnStatusChanged);
            _acceptBtn.onClick.RemoveListener(HideWindow);
        }

        #endregion Unity

        #region Event Callbacks

        private void OnStatusChanged(MqttConnectionStatusData statusData)
        {
            _title.text = statusData.Title;
            _message.text = statusData.Message;

            if (statusData.Status == MqttConnectionStatus.Connecting) _acceptBtn.gameObject.SetActive(false);
            else _acceptBtn.gameObject.SetActive(true);

            _canvas.enabled = true;
        }

        #endregion Event Callbacks
    }
}