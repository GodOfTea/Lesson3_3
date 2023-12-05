using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Visitor
{
    public class Spawner: MonoBehaviour, IEnemyDeathNotifier
    {
        public event Action<Enemy> Notified;

        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] protected float _spawnCooldown;

        protected SpawnerStatus _status;
        private List<Enemy> _spawnedEnemies = new List<Enemy>();
        private Coroutine _spawn;
        
        public virtual void Init() { }
        
        public void StartWork()
        {
            StopWork();

            _status = SpawnerStatus.Work;
            _spawn = StartCoroutine(Spawn());
        }

        public void StopWork()
        {
            if (_spawn != null)
            {
                StopCoroutine(_spawn);
            }
            
            _status = SpawnerStatus.Stop;
        }

        public void KillRandomEnemy()
        {
            if (_spawnedEnemies.Count == 0)
                return;

            _spawnedEnemies[UnityEngine.Random.Range(0, _spawnedEnemies.Count)].Kill();
        }

        protected virtual IEnumerator Spawn()
        {
            while (true)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_spawnCooldown);
            }
        }

        protected void SpawnEnemy()
        {
            Enemy enemy = _enemyFactory.Get((EnemyType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyType)).Length));
            enemy.MoveTo(_spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position);
            enemy.Died += OnEnemyDied;
            _spawnedEnemies.Add(enemy);
            HandleDependenciesWithEnemy(enemy);
        }

        /* Не смог придумать названия лучше (: */
        protected virtual void HandleDependenciesWithEnemy(Enemy enemy) { }

        protected virtual void OnEnemyDied(Enemy enemy)
        {
            Notified?.Invoke(enemy);
            enemy.Died -= OnEnemyDied;
            _spawnedEnemies.Remove(enemy);
        }

        /* Возможно задача в том, чтоб добавить сюда похожий визитер, но все мои попытки выглядят скорее как костыль,
         чем как решение. Я не могу понять, кто должен сообщать спавнеру, чтоб он добавил вес? Сам враг? 
         Мне кажется, что отслеживание и остановка работы, если собледены условия по весу, это полностью ответственность
         спавнера и доп. классов, который находятся в нем. */
        
        // private class EnemyVisitor : IEnemyVisitor
        // {
        //     private EnemyWeightHandler _weightHandler;
        //     
        //     public EnemyVisitor(EnemyWeightHandler weightHandler)
        //     {
        //         _weightHandler = weightHandler;
        //     }
        //
        //     public void Visit(Ork ork) => _weightHandler.AddWeight(ork.Data.Weight);
        //     public void Visit(Human human) => _weightHandler.AddWeight(human.Data.Weight);
        //     public void Visit(Elf elf) => _weightHandler.AddWeight(elf.Data.Weight);
        //     public void Visit(Enemy enemy) => Visit((dynamic)enemy);
        // }
    }
}
