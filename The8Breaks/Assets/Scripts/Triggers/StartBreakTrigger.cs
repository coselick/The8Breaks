using UnityEngine;

namespace MBW.The8Breaks
{
    public class StartBreakTrigger : MonoBehaviour
    {
        [SerializeField] private Director _director;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            _director.StartBreak();
            gameObject.SetActive(false);
        }
    }
}
