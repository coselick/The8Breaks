using Hertzole.GoldPlayer;
using System.Collections.Generic;
using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class LiftTrigger : MonoBehaviour
    {
        [SerializeField] private float _y;
        [SerializeField] private List<GameObject> _objs2Activate, _objs2Deactivate;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            GoldPlayerController player = other.GetComponent<GoldPlayerController>();
            player.SetPosition(other.transform.position + new Vector3(0f, _y, 0f));
            //CharacterController cc = other.GetComponent<CharacterController>();
            //cc.enabled = false;
            //other.transform.position += new Vector3(0f, _y, 0f);
            //cc.enabled = true;
            foreach (GameObject trigger in _objs2Activate) trigger.SetActive(true);
            foreach (GameObject trigger in _objs2Deactivate) trigger.SetActive(false);
        }
    }
}