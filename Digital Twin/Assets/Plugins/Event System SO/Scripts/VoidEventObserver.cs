using System;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Event_System_SO.Scripts
{
    public class VoidEventObserver : MonoBehaviour
    {
        [SerializeField]
        private VoidEventSO voidEvent;

        [SerializeField]
        private UnityEvent response;

        private void OnEnable() => voidEvent.AddObserver(OnEventRaised);

        private void OnDisable() => voidEvent.RemoveObserver(OnEventRaised);

        private void OnEventRaised() => response.Invoke();
    }
}
