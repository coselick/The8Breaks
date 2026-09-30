using UnityEngine;

namespace MBW.The8Breaks.Triggers
{
    public class HideTriggers : MonoBehaviour
    {
        void Start() { foreach (var trigger in GetComponentsInChildren<MeshRenderer>(true)) trigger.enabled = false; }
    }
}
