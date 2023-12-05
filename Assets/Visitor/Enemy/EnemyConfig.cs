using System;
using UnityEngine;

namespace Assets.Visitor
{
    [Serializable]
    public class EnemyConfig
    {
        [SerializeField] private float _weight;

        public float Weight => _weight;
    }
}