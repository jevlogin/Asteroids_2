using System;
using UnityEngine;

namespace WORLDGAMEDEVELOPMENT
{
    internal class UnityMessageManager
    {
        private static UnityMessageManager instance;

        public event Action<MessageHandler> OnMessage;

        private UnityMessageManager() { }

        public static UnityMessageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnityMessageManager();
                }
                return instance;
            }
        }

        public void ReceiveMessage(string source, string data)
        {
            OnMessage?.Invoke(new MessageHandler(source, data));
            Debug.Log($"Выполнение ReceiveMessage - {source}-{data}");
        }
    }
}