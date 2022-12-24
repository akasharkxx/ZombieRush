using DG.Tweening;
using Redcode.Pools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace ZombieRush
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] internal EnemyType _type;

        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _rotateSpeed = 10f;
        [SerializeField] private Transform _rotator;
        [SerializeField] protected Transform _model;

        private NavMeshAgent _agent;
        private Transform _target;

        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _damageToPlayer = 10;
        private int _currentHealth = 0;

        private bool _isDead = false;

        [SerializeField] private MeshRenderer[] meshRenderers;
        [SerializeField, ColorUsage(true, true)] private Color hitColor;

        internal IObjectPool<Enemy> _pool;
        internal Pool<Enemy> _enemyPool;

        public bool IsDead { get => _isDead; private set => _isDead = value; }

        internal delegate void EnemyAction();
        internal delegate void EnemyActionInt(int value);

        internal event EnemyAction OnDied;
        internal event EnemyActionInt OnTakeDamage;

        //internal void SetPool(IObjectPool<Enemy> pool)
        //{
        //    _pool = pool;
        //    _isDead = true;
        //}

        internal void SetPool(Pool<Enemy> pool)
        {
            _enemyPool = pool;
            IsDead = true;
        }

        internal virtual void Init(Vector3 startPosition, EnemyType type = EnemyType.Zombie)
        {
            transform.position = startPosition;

            _target = GameManager.Instance.playerTransform;
            IsDead = false;
            _model.gameObject.SetActive(true);
            _currentHealth = _maxHealth;

            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _movementSpeed;
            _agent.angularSpeed = _rotateSpeed;

        }

        private void FixedUpdate()
        {
            if(IsDead) { return; }

            _agent.SetDestination(_target.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Bullet"))
            {
                TakeDamage(_damageToPlayer);
            }
        }

        internal virtual void TakeDamage(int damage)
        {
            if (IsDead)
            {
                return;
            }
            
            _currentHealth -= damage;
            OnTakeDamage?.Invoke(damage);

            if(_currentHealth <= 0)
            {
                EnemyDead();
                return;
            }
            TakeHitVisual();
        }

        private void EnemyDead()
        {
            IsDead = true;
            _currentHealth = 0;
            OnDied?.Invoke();
            GameManager.Instance.EnemyDied();
        }

        internal virtual void DeadCleanup()
        {
            //_pool.Release(this);
            _enemyPool.Take(this);
        }

        internal void TakeHitVisual()
        {
            for(int i = 0; i < meshRenderers.Length; i++)
            {                
                meshRenderers[i].material.DOColor(hitColor, 0.04f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo);
            }
        }
    }
}

