using System.Collections.Generic;
using UnityEngine;
using Hertzole.GoldPlayer;

namespace MBW.The8Breaks.Triggers
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _point;
        [SerializeField] private List<GameObject> _triggers2Activate, _triggers2Deactivate;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            GoldPlayerController player = other.GetComponent<GoldPlayerController>();
            Vector3 localOffset = transform.InverseTransformPoint(other.transform.position); localOffset.z = 0f;
            Vector3 newPosition = _point.TransformPoint(localOffset);
            Quaternion relativeRotation = Quaternion.Inverse(transform.rotation) * other.transform.rotation;
            Quaternion newRotation = _point.rotation * relativeRotation;
            float newYRotation = newRotation.eulerAngles.y;
            player.SetPositionAndRotation(newPosition, newYRotation);
            player.HeadBob.ResetBob();
            player.Audio.ResetAudio();
            foreach (GameObject trigger in _triggers2Activate) trigger.SetActive(true);
            foreach (GameObject trigger in _triggers2Deactivate) trigger.SetActive(false);
        }
    }
}