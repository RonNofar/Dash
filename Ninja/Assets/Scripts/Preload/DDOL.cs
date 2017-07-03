using UnityEngine;

namespace Ninja.Preload
{
    public class DDOL : MonoBehaviour
    {

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
