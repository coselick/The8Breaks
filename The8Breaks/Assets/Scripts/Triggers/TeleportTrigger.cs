using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private bool _addY;
        [SerializeField] private float _y;
        [SerializeField] private Transform _point;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (_addY) other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + _y, other.transform.position.z);
            else other.transform.position = _point.position;
        }
    }
}
