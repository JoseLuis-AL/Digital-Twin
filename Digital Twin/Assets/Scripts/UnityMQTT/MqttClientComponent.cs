using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMQTT.Events;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace UnityMQTT
{
    public abstract class MqttClientComponent : MonoBehaviour
    {
        [Header("MQTT Broker Configuration")] 
        [SerializeField] protected string brokerAddress = "127.0.0.1";
        
        [SerializeField] protected int brokerPort = 1883;
        [SerializeField] protected bool autoConnect;

        [Header("Connection Parameters")] 
        [SerializeField] private int connectionDelay = 500;

        [SerializeField] 
        private int timeoutOnConnection = MqttSettings.MQTT_CONNECT_TIMEOUT;

        private readonly List<MqttMsgPublishEventArgs> messageQueue1 = new();
        private readonly List<MqttMsgPublishEventArgs> messageQueue2 = new();
        private List<MqttMsgPublishEventArgs> frontMessageQueue;
        private List<MqttMsgPublishEventArgs> backMessageQueue;

        protected MqttClient client;

        #region Unity methods

        protected virtual void Awake()
        {
            frontMessageQueue = messageQueue1;
            backMessageQueue = messageQueue2;
        }

        protected virtual void Start()
        {
            if (autoConnect) Connect();
        }

        protected virtual void Update()
        {
            ProcessMqttEvents();
        }

        protected virtual void OnDisable()
        {
            Disconnect();
        }

        #endregion

        #region MQTT connection methods

        public void Connect()
        {
            if (client == null || !client.IsConnected) StartCoroutine(ConnectCoroutine());
        }

        public void Disconnect()
        {
            if (client != null) CloseConnection();
        }

        protected IEnumerator ConnectCoroutine()
        {
            // Wait for the given delay.
            yield return new WaitForSecondsRealtime(connectionDelay / 1000f);

            // Leave some time to Unity for process.
            yield return new WaitForEndOfFrame();

            // Create the client instance.
            if (client != null && client.IsConnected) yield break;
            if (client == null)
            {
                try
                {
                    client = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);
                }
                catch (Exception e)
                {
                    client = null;
                    OnConnectionFailed(e.Message);
                    yield break;
                }
            }

            OnConnecting();

            // Leave some time to Unity to process.
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // Connect to the broker.
            client.Settings.TimeoutOnConnection = timeoutOnConnection;
            var clientId = Guid.NewGuid().ToString();
            try
            {
                client.Connect(clientId);
            }
            catch (Exception e)
            {
                client = null;
                OnConnectionFailed(e.Message);
                yield break;
            }

            // Connection established.
            if (client.IsConnected)
            {
                client.ConnectionClosed += OnConnectionClosed;
                client.MqttMsgPublishReceived += OnMqttMessageReceived;
                OnConnected();
            }
            else
            {
                OnConnectionFailed("Connection Failed.");
            }

            Debug.Log("Connected.");
        }

        private void CloseConnection()
        {
            // Check if the client exist or if the client is connected.
            if (client == null || !client.IsConnected) return;

            // Disconnect from the broker.
            UnsubscribeTopics();
            client.Disconnect();

            // Remove callbacks.
            client.ConnectionClosed -= OnConnectionClosed;
            client.MqttMsgPublishReceived -= OnMqttMessageReceived;
            client = null;

            OnConnectionClose();
        }

        #endregion

        #region MQTT event methods

        private void OnConnectionClosed(object sender, EventArgs msg)
        {
#if UNITY_EDITOR
            Debug.Log("Connection closed.");
#endif
        }

        private void OnMqttMessageReceived(object sender, MqttMsgPublishEventArgs msg)
        {
            frontMessageQueue.Add(msg);
        }

        #endregion

        #region MQTT connection status methods

        protected virtual void OnConnecting()
        {
#if UNITY_EDITOR
            Debug.Log($"Connecting to {brokerAddress}:{brokerPort}.");
#endif
        }

        protected virtual void OnConnected()
        {
#if UNITY_EDITOR
            Debug.Log($"Connected to {brokerAddress}:{brokerPort}.");
#endif
            SubscribeTopics();
        }

        protected virtual void OnConnectionLost()
        {
#if UNITY_EDITOR
            Debug.Log("Connection Lost!");
#endif
        }

        protected virtual void OnConnectionFailed(string errorMessage)
        {
#if UNITY_EDITOR
            Debug.Log($"Connection Failed on {brokerAddress}:{brokerPort}: {errorMessage}");
#endif
        }

        protected virtual void OnConnectionClose()
        {
#if UNITY_EDITOR
            Debug.Log("Connection closed.");
#endif
        }

        #endregion

        #region MQTT topic methods

        protected abstract void SubscribeTopics();

        protected abstract void UnsubscribeTopics();

        #endregion

        #region MQTT message processing methods

        protected virtual void DecodeMessage(string topic, byte[] message)
        {
            Debug.Log($"Message received on topic {topic}.");
        }

        private void SwapMessageQueues()
        {
            if (frontMessageQueue == messageQueue1)
            {
                frontMessageQueue = messageQueue2;
                backMessageQueue = messageQueue1;
            }
            else
            {
                frontMessageQueue = messageQueue1;
                backMessageQueue = messageQueue2;
            }
        }

        private void ProcessBackgroundQueue()
        {
            foreach (var msg in backMessageQueue)
            {
                DecodeMessage(msg.Topic, msg.Message);
            }
            
            backMessageQueue.Clear();
        }

        protected virtual void ProcessMqttEvents()
        {
            // Process messages in the main queue.
            SwapMessageQueues();
            ProcessBackgroundQueue();
            
            // Process messages in the background queue.
            SwapMessageQueues();
            ProcessBackgroundQueue();
        }

        #endregion
    }
}