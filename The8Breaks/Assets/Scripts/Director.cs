using System;
using System.Collections.Generic;
using UnityEngine;

namespace MBW.The8Breaks
{
    public enum AnomalyType
    {
        Passive,
        Active,
        Event
    }
    public class Director : MonoBehaviour
    {
        [HideInInspector] public int _currentBreak;
        [HideInInspector] public bool _isItAnomaly;
        [Range(0f, 1f)][SerializeField] private float _initAnomalyChance;
        [Range(1, 16)][SerializeField] private int _anomaliesPerBreak;
        [SerializeField] private List<AnomalyList> _anomalies;
        private List<Anomaly> _usedAnomalies;
        private float _anomalyChance;

        private void Start()
        {
            _anomalyChance = _initAnomalyChance;
        }

        private void Update()
        {

        }

        public void StartBreak()
        {
            if (_currentBreak == 8) Debug.Log("game won"); return;
            _isItAnomaly = UnityEngine.Random.Range(0f, 1f) < _anomalyChance ? true : false;
            if (_isItAnomaly)
            {
                _anomalyChance *= 0.75f;
                if (_anomalyChance < 0) _anomalyChance = 0;
            }
            else
            {
                _anomalyChance *= 1.2f;
                if (_anomalyChance > 1) _anomalyChance = 1;
            }
            
        }
    }
    [Serializable] public class AnomalyList
    {
        public AnomalyType Type;
        [Range(0f, 1f)] public float Chance;
        [Range(0f, 1f)] public float IsNextBreakIsAnomalyChanceDegrade;
        public List<Anomaly> _List;
    }
}