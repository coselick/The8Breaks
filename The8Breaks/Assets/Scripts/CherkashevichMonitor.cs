using System.Collections.Generic;
using UnityEngine;

namespace MBW.The8Breaks
{
    public class CherkashevichMonitor : MonoBehaviour
    {
        [SerializeField] private List<Material> _materials;
        private Renderer _renderer;

        public void ChangeMaterial(int index)
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            _renderer.material = _materials[index];
        }
    }
}
