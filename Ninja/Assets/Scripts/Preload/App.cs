using UnityEngine;

namespace Ninja.Preload
{
    public class App : MonoBehaviour
    {
        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
