using UnityEngine;

namespace MBW.The8Breaks.Anomalies
{
    [CreateAssetMenu(fileName = "New Anomaly", menuName = "Anomaly")]
    public class Anomaly : ScriptableObject
    {
        public AnomalyType Type;
        public string NameKey, DescriptionKey;
        public Sprite Icon;
    }
}