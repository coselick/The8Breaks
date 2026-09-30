using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class DoorCloseTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
        }
    }
}
