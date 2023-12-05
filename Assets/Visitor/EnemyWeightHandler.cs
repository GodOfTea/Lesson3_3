using System;
using UnityEngine;

namespace Assets.Visitor
{
    public class EnemyWeightHandler
    {
        private float _maxWeight;
        private float _currentWeight;

        public event Action GotAvailableSpace;
        public event Action MaxWeightReached;

        public EnemyWeightHandler(float maxWight)
        {
            _maxWeight = maxWight;
            _currentWeight = 0;
        }

        public void AddWeight(float value)
        {
            if (_currentWeight > _maxWeight)
                throw new Exception("Попытка привысить максимльно допустимый вес!!");
            
            _currentWeight += value;

            if (_currentWeight >= _maxWeight)
                MaxWeightReached?.Invoke();
            
            Log("Plus");   
        }

        public void RemoveWeight(float value)
        {
            _currentWeight -= value;

            if (_currentWeight < 0)
                _currentWeight = 0;
            
            if (_currentWeight < _maxWeight)
                GotAvailableSpace?.Invoke();
            
            Log("Minus");
        }

        private void Log(string operation)
        {
            Debug.Log($"{operation}. Current wight: {_currentWeight}/{_maxWeight}");
        }
    }
}