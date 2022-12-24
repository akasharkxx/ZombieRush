using System;
using UnityEngine;
using Redcode.Pools;

namespace ZombieRush
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPair[] _enemyList;
        [SerializeField] private float _spawnInterval = 1.0f;
        [SerializeField, Range(1, 200)] private float _spawnInnerRadius = 1.0f;
        [SerializeField, Range(1, 200)] private float _spawnOuterRadius = 1.0f;
        [SerializeField] private bool _canSpawn = false;
        [SerializeField, Range(10, 300)] private int _pooledEnemyCount;

        [SerializeField] private Transform _defaultStartPosition;

        private Pool<Enemy> _pool;

        private float _tempTimer = 0f;

        private void Awake()
        {
            _pool = Pool.Create(_enemyList[0].enemy, _pooledEnemyCount, transform);
        }

        private void Update()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            if (!_canSpawn) return;
            _tempTimer += Time.deltaTime;

            if (_tempTimer > _spawnInterval)
            {
                _tempTimer = 0f;

                float angle = UnityEngine.Random.Range(0, MathF.PI * 2);
                float radius = UnityEngine.Random.Range(_spawnInnerRadius, _spawnOuterRadius);

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                Vector3 positionToSpawn = new Vector3(x, 0.5f, z);

                positionToSpawn += GameManager.Instance.playerTransform.position;
                positionToSpawn.y = 0.5f;

                var enemy = _pool.Get();
                enemy.SetPool(_pool);
                enemy.Init(positionToSpawn);
            }
        }

        private Enemy CreateNewEnemy()
        {
            int index = 0;

            var enemy = Instantiate(_enemyList[index].enemy, _defaultStartPosition.position, Quaternion.identity, transform);
            //enemy.SetPool(_enemyPool);
            //enemy.SetPool(_pool);
            return enemy;
        }

        private void TakeEnemyFromPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void OnReturnEnemyToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(GameManager.Instance.playerTransform.position, _spawnInnerRadius);
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(GameManager.Instance.playerTransform.position, _spawnOuterRadius);
        //}
    }

    [Serializable]
    internal struct EnemyPair
    {
        [SerializeField] internal EnemyType enemyType;
        [SerializeField] internal Enemy enemy;
    }

    internal enum EnemyType
    {
        Zombie
    }
}

