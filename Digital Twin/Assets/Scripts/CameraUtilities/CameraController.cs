using UnityEngine;

namespace CameraUtilities
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Target the camera is looking at.
        /// </summary>
        [Header("Look At")]
        [SerializeField] private Transform _target;

        /// <summary>
        /// Camera reference.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Previous camera position.
        /// </summary>
        private Vector3 _prevPosition;

        /// <summary>
        /// Get the camera reference.
        /// </summary>
        private void Start() => _camera = GetComponent<Camera>();

        /// <summary>
        /// Update the camera position
        /// </summary>
        private void Update()
        {
            // Get the previous camera position.
            if (Input.GetMouseButtonDown(0))
                _prevPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

            // Update the camera position when the mouse is pressed.
            if (Input.GetMouseButton(0)) UpdateCameraPosition();
        }

        /// <summary>
        /// Update the position and rotation of the camera to observe the target
        /// using the mouse position to calculate the direction.
        /// </summary>
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
    }
}