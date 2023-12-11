using UnityEngine;

namespace UI.ModalWindow
{
    [RequireComponent(typeof(Canvas))]
    public class ModalWindow : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] protected Canvas _canvas;

        public void ShowWindow() => _canvas.enabled = true;

        public void HideWindow() => _canvas.enabled = false;
    }
}