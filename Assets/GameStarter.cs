using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class GameStarter : MonoBehaviour
    {
        public event Action OnStartGame;

        public void StartGame()
        {
            Debug.Log("Игра начата! StartGame method");
            OnStartGame?.Invoke();
        }

        public void StartGame2()
        {
            Debug.Log("this is strange method StartGame2");
        }

        public void NewMethod(string data)
        {
            Debug.Log("NewMethod method");
            if(data == "StartGame")
            {
                StartGame();
            }
        }

        void Start()
        {
            UnityMessageManager.Instance.OnMessage += OnUnityMessage;
        }

        void OnUnityMessage(MessageHandler handler)
        {
            if (handler.Data == "StartGame" && handler.Source == "GameController")
            {
                StartGame();
            }
        }

    }
}