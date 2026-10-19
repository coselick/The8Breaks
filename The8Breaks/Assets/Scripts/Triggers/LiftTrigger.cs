using UnityEngine;
using Hertzole.GoldPlayer;

namespace MBW.The8Breaks.Triggers
{
    public class LiftTrigger : MonoBehaviour
    {
        [SerializeField] private float _y;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            GoldPlayerController player = other.GetComponent<GoldPlayerController>();
            player.SetPosition(other.transform.position + new Vector3(0f, _y, 0f));
            //CharacterController cc = other.GetComponent<CharacterController>();
            //cc.enabled = false;
            //other.transform.position += new Vector3(0f, _y, 0f);
            //cc.enabled = true;
        }
    }
}