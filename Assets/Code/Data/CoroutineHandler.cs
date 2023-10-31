using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class CoroutineHandler : MonoBehaviour
    {
        private static CoroutineHandler instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static CoroutineHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("CoroutineHandler");
                    instance = go.AddComponent<CoroutineHandler>();
                }
                return instance;
            }
        }
    }
}