using System.Collections;
using UnityEngine;

namespace Assets.Visitor
{
    public class SpawnerByWeight : Spawner
    {
        [Header("Data")]
        [SerializeField] private SpawnerConfig _spawnerConfig;

        private EnemyWeightHandler _weightHandler;
        
        public override void Init()
        {
            _weightHandler = new EnemyWeightHandler(_spawnerConfig.MaxWeight);

            _weightHandler.MaxWeightReached += OnMaxWeightReached;
            _weightHandler.GotAvailableSpace += OnGotAvailableSpace;
        }
        
        private void OnDestroy()
        {
            _weightHandler.MaxWeightReached -= OnMaxWeightReached;
            _weightHandler.GotAvailableSpace -= OnGotAvailableSpace;
        }

        protected override IEnumerator Spawn()
        {
            while (true)
            {
                if (_status != SpawnerStatus.Pause)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(_spawnCooldown);
                }
                else
                    yield return new WaitForEndOfFrame();
            }
        }

        protected override void OnEnemyDied(Enemy enemy)
        {
            base.OnEnemyDied(enemy);
            _weightHandler.RemoveWeight(enemy.Data.Weight);
        }

        protected override void HandleDependenciesWithEnemy(Enemy enemy)
        {
            _weightHandler.AddWeight(enemy.Data.Weight);
        }

        private void OnMaxWeightReached()
        {
            _status = SpawnerStatus.Pause;
        }

        private void OnGotAvailableSpace()
        {
            _status = SpawnerStatus.Work;
        }
    }
}