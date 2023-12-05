using System;
using UnityEngine;

namespace Assets.Visitor
{
    [Serializable]
    public abstract class Enemy: MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        
        public EnemyConfig Data => _enemyConfig;
        
        public event Action<Enemy> Died;
        //Какая то общая логика врага: передвижение, жизни и тп.

        public void MoveTo(Vector3 position) => transform.position = position;

        public void Kill()
        {
            Died?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
