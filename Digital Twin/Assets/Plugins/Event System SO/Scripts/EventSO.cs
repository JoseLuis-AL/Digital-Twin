using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Event_System_SO.Scripts
{
    public abstract class EventSO<T> : ScriptableObject
    {
        private readonly UnityEvent<T> _onRaiseEvent = new UnityEvent<T>();

        public void Invoke(T value) => _onRaiseEvent?.Invoke(value);

        public void AddListener(UnityAction<T> call) => _onRaiseEvent.AddListener(call);

        public void RemoveListener(UnityAction<T> call) => _onRaiseEvent.RemoveListener(call);
    }
}