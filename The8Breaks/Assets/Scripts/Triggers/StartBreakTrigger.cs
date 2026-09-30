using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class StartBreakTrigger : MonoBehaviour
    {
        [SerializeField] private Director _director;
        [SerializeField] private GameObject _liftTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            _director.StartBreak();
            _liftTrigger.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
