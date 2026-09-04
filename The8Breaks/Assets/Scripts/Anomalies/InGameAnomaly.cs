using UnityEngine;

namespace MBW.The8Breaks.Anomalies
{
    public abstract class InGameAnomaly : MonoBehaviour
    {
        public string NameKey;
        [SerializeField] protected GameObject _normalVersion;
        [SerializeField] protected GameObject _anomalyVersion;
        private Vector3 _initPosition;
        private Quaternion _initRotation;

        protected virtual void Start()
        {
            _initPosition = transform.position;
            _initRotation = transform.rotation;
            DeactivateAnomaly();
        }
        public virtual void ActivateAnomaly() { ResetTransforms(); SetActiveState(true); }

        public virtual void DeactivateAnomaly() { ResetTransforms(); SetActiveState(false); }

        private void SetActiveState(bool isAnomaly) { _normalVersion.SetActive(!isAnomaly); _anomalyVersion.SetActive(isAnomaly); }

        private void ResetTransforms()
        {
            _normalVersion.transform.position = _initPosition; _normalVersion.transform.rotation = _initRotation;
            _anomalyVersion.transform.position = _initPosition; _anomalyVersion.transform.rotation = _initRotation;
        }
    }
}