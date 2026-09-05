using MBW.The8Breaks.Anomalies;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject _inGameAnomaliesParent;
        private List<Anomaly> _usedAnomalies;
        private float _anomalyChance;

        private void Start()
        {
            _anomalyChance = _initAnomalyChance;
            _usedAnomalies = new List<Anomaly>();
            StartBreak();
        }
        public void StartBreak()
        {
            if (_currentBreak == 8) { Debug.Log("game won"); return; }
            _isItAnomaly = UnityEngine.Random.Range(0f, 1f) < _anomalyChance;
            if (_isItAnomaly)
            {
                _anomalyChance *= 0.75f;
                List<Anomaly> selected = SelectAnomalies();
                var usedTypes = new HashSet<AnomalyType>();
                foreach (var anomaly in selected)
                {
                    AnomalyList list = _anomalies.Find(x=>x._List.Contains(anomaly));
                    if (list != null && usedTypes.Add(list.Type)) _anomalyChance *= (1f - list.IsNextBreakIsAnomalyChanceDegrade);
                    _inGameAnomaliesParent.GetComponentsInChildren<InGameAnomaly>().FirstOrDefault(x => x.name == anomaly.NameKey).ActivateAnomaly();
                }
                _usedAnomalies.AddRange(selected);
            }
            else
            {
                _anomalyChance *= 1.2f;
                if (_anomalyChance > 1f) _anomalyChance = 1f;
            }
            if (_anomalyChance < 0f) _anomalyChance = 0f;
            _currentBreak++;
        }

        private List<Anomaly> SelectAnomalies()
        {
            var selected = new List<Anomaly>();
            bool eventSelected = false;
            for (int i = 0; i < _anomaliesPerBreak; i++)
            {
                var validLists = new List<AnomalyList>();
                foreach (var list in _anomalies)
                {
                    if (list.Type == AnomalyType.Event && eventSelected) continue;
                    bool notUsedPesda = list._List.Any(x=>!_usedAnomalies.Contains(x) && !selected.Contains(x));
                    if (notUsedPesda) validLists.Add(list);
                }

                if (validLists.Count == 0) break;
                float totalChance = 0f;
                foreach (var l in validLists) totalChance += l.Chance;
                float random = UnityEngine.Random.Range(0f, totalChance);
                float yaUzheNeZnauKakNazivatEtiKonchenniePeremennie = 0f;
                AnomalyList chosenList = validLists[0];
                foreach (var l in validLists)
                {
                    yaUzheNeZnauKakNazivatEtiKonchenniePeremennie += l.Chance;
                    if (random <= yaUzheNeZnauKakNazivatEtiKonchenniePeremennie) { chosenList = l; break; }
                }
                var available = chosenList._List.Where(x => !_usedAnomalies.Contains(x) && !selected.Contains(x)).ToList();
                if (available.Count > 0)
                {
                    Anomaly chosen = available[UnityEngine.Random.Range(0, available.Count)];
                    selected.Add(chosen);
                    if (chosenList.Type == AnomalyType.Event) eventSelected = true;
                }
            }
            return selected;
        }
    }

    [Serializable]
    public class AnomalyList
    {
        public AnomalyType Type;
        [Range(0f, 1f)] public float Chance;
        [Range(0f, 1f)] public float IsNextBreakIsAnomalyChanceDegrade;
        public List<Anomaly> _List;
    }
}