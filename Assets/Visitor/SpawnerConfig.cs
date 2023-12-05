using System;
using UnityEngine;

namespace Assets.Visitor
{
    [Serializable]
    public class SpawnerConfig
    {
        [SerializeField] private float _maxWeight;

        public float MaxWeight => _maxWeight;
    }
}