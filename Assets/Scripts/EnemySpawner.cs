using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

namespace ZombieRush
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPair[] _enemyList;
        [SerializeField] private float _spawnInterval = 1.0f;
        [SerializeField] private float _spawnRadius = 1.0f;
        [SerializeField] private bool _canSpawn = false;

        [SerializeField] private Transform _defaultStartPosition;

        private IObjectPool<Enemy> _enemyPool;

        private float _tempTimer = 0f;

        private void Awake()
        {
            _enemyPool = new ObjectPool<Enemy>(CreateNewEnemy, TakeEnemyFromPool, OnReturnEnemyToPool);
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

                Vector3 startPosition = UnityEngine.Random.insideUnitSphere * _spawnRadius;
                startPosition += GameManager.Instance.playerTransform.position;
                startPosition.y = 0.5f;
                var enemy = _enemyPool.Get();
                enemy.Init(startPosition);
            }
        }

        private Enemy CreateNewEnemy()
        {
            int index = 0;

            var enemy = Instantiate(_enemyList[index].enemy, _defaultStartPosition.position, Quaternion.identity, transform);
            enemy.SetPool(_enemyPool);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
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

