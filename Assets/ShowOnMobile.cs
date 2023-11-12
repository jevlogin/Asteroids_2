using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class ShowOnMobile : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(Application.isMobilePlatform);
        }
    }
}