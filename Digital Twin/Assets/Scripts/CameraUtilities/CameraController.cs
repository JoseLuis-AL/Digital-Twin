using UnityEngine;
using UnityMQTT.Connection;
using UnityMQTT.Events;

namespace CameraUtilities
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Look At")]
        [SerializeField] private Transform _target;

        [Header("Events SO")]
        [SerializeField] private MqttConnectionStatusEvtSO _connectionStatusEvtSO;

        private bool _canMove = false;
        private Camera _camera;
        private Vector3 _prevPosition;

        #region Unity

        private void Start() => _camera = GetComponent<Camera>();

        private void OnEnable() => _connectionStatusEvtSO.AddObserver(OnConnectionStatusChanged);

        private void OnDisable() => _connectionStatusEvtSO.AddObserver(OnConnectionStatusChanged);

        private void Update()
        {
            if (!_canMove) return;

            // Get the previous camera position.
            if (Input.GetMouseButtonDown(0))
                _prevPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

            // Update the camera position when the mouse is pressed.
            if (Input.GetMouseButton(0)) UpdateCameraPosition();
        }

        #endregion Unity

        #region Camera Movement

        private void UpdateCameraPosition()
        {
            var direction = _prevPosition - _camera.ScreenToViewportPoint(Input.mousePosition);

            // Focus the target.
            _camera.transform.position = _target.position;

            // Rotate the camera towards the target.
            _camera.transform.Rotate(new Vector3(1, 0, 0), direction.y * 100);
            _camera.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            _camera.transform.Translate(new Vector3(0, 0, -50));

            // Get the camera previous position.
            _prevPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }

        #endregion Camera Movement

        #region Connection Status

        private void OnConnectionStatusChanged(MqttConnectionStatusData statusData)
        {
            _canMove = statusData.Status == UnityMQTT.MqttConnectionStatus.Connected;
        }

        #endregion Connection Status
    }
}