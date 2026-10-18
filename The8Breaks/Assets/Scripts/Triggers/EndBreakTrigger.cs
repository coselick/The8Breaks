using UnityEngine;

namespace MBW.The8Breaks
{
    public class EndBreakTrigger : MonoBehaviour
    {
        [SerializeField] private Director _director;
        [SerializeField] private bool _backrooms = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            _director.EndBreak(_backrooms);
        }
    }
}
