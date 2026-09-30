using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class LiftTrigger : MonoBehaviour
    {
        [SerializeField] private float _y;
        [SerializeField] private bool _deactivate;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + _y, other.transform.position.z);
            if (_deactivate) gameObject.SetActive(false);
        }
    }
}
