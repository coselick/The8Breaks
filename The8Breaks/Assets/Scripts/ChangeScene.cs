using UnityEngine;

namespace Q17pD
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private int _sceneID;
        public void LoadScene() { UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneID); }
    }
}
