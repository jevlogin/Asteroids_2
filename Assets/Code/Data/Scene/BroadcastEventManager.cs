using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class BroadcastEventManager : MonoBehaviour
    {
        #region Fields
        
        public event Action OnStartGame; 

        #endregion


        public void StartGame()
        {
            Debug.Log($"StartGame");
            OnStartGame?.Invoke();
        }
    }
}