using UnityEngine;
using UnityEngine.Video;

namespace MBW.The8Breaks.Anomalies
{
    public class TV : InGameAnomaly
    {
        protected override void SetActiveState(bool isAnomaly)
        {
            base.SetActiveState(isAnomaly);
            _normalVersion.GetComponent<VideoPlayer>().Play(); _anomalyVersion.GetComponent<VideoPlayer>().Play();
        }
    }
}
