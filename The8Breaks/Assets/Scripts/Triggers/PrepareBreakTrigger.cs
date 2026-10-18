using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class PrepareBreakTrigger : MonoBehaviour
    {
        [SerializeField] private Director _director;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            _director.PrepareBreak();
            gameObject.SetActive(false);
        }
    }
}
