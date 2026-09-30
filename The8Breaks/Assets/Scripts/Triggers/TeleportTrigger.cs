using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        [SerializeField] private GameObject _startTrigger, _liftTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
            localOffset.z = 0f;
            Vector3 newPosition = _point.TransformPoint(localOffset);
            Quaternion relativeRotation = Quaternion.Inverse(transform.rotation) * other.transform.rotation;
            Quaternion newRotation = _point.rotation * relativeRotation;
            other.transform.SetPositionAndRotation(newPosition, newRotation);
            Rigidbody rb = other.attachedRigidbody;
            Quaternion deltaRot = _point.rotation * Quaternion.Inverse(transform.rotation);
            rb.velocity = deltaRot * rb.velocity;
            rb.angularVelocity = deltaRot * rb.angularVelocity;
            rb.position = newPosition; rb.rotation = newRotation;
            _startTrigger.gameObject.SetActive(true);
            _liftTrigger.gameObject.SetActive(false);
        }
    }
}