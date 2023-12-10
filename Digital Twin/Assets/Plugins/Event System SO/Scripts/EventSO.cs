using UnityEngine;
using UnityEngine.Events;

namespace Plugins.Event_System_SO.Scripts
{
    public abstract class EventSO<T> : ScriptableObject
    {
        private readonly UnityEvent<T> _onRaiseEvent = new UnityEvent<T>();

        public void Invoke(T value) => _onRaiseEvent?.Invoke(value);

        public void AddObserver(UnityAction<T> call) => _onRaiseEvent.AddListener(call);

        public void RemoveObserver(UnityAction<T> call) => _onRaiseEvent.RemoveListener(call);
    }

    public abstract class EventSO<T, T2> : ScriptableObject
    {
        private readonly UnityEvent<T, T2> _onRaiseEvent = new UnityEvent<T, T2>();

        public void Invoke(T value, T2 value2) => _onRaiseEvent?.Invoke(value, value2);

        public void AddObserver(UnityAction<T, T2> call) => _onRaiseEvent.AddListener(call);

        public void RemoveObserver(UnityAction<T, T2> call) => _onRaiseEvent.RemoveListener(call);
    }

    public abstract class EventSO<T, T2, T3> : ScriptableObject
    {
        private readonly UnityEvent<T, T2, T3> _onRaiseEvent = new UnityEvent<T, T2, T3>();

        public void Invoke(T value, T2 value2, T3 value3) => _onRaiseEvent?.Invoke(value, value2, value3);

        public void AddObserver(UnityAction<T, T2, T3> call) => _onRaiseEvent.AddListener(call);

        public void RemoveObserver(UnityAction<T, T2, T3> call) => _onRaiseEvent.RemoveListener(call);
    }
}