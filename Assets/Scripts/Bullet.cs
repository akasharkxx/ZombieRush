using DG.Tweening;
using UnityEngine;
using Redcode.Pools;

namespace ZombieRush
{

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;

        private Rigidbody _rigidBody;
        private float _bulletSpeed = 1f;

        //private IObjectPool<Bullet> _pool;
        private Pool<Bullet> _bulletPool;
        private Tween returnToPoolTween;

        internal Transform Transform { get; private set; }

        private Vector3 _startPosition;

        private void Awake()
        {
            Transform = transform;
        }

        //internal void SetPool(IObjectPool<Bullet> pool)
        //{
        //    _pool = pool;
        //}

        internal void SetPool(Pool<Bullet> pool)
        {
            _bulletPool = pool;
        }

        internal void Init(Vector3 startPosition, float bulletSpeed = 1f)
        {
            _startPosition = startPosition;
            _bulletSpeed = bulletSpeed;

            Transform.position = _startPosition;
            
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.AddForce(Transform.forward * _bulletSpeed, ForceMode.Impulse);

            trailRenderer.emitting = true;

            if(returnToPoolTween != null ) { returnToPoolTween.Kill(); }

            returnToPoolTween = DOVirtual.DelayedCall(5f, ReturnToPool);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(returnToPoolTween != null) { returnToPoolTween.Kill(); }
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            trailRenderer.Clear();
            trailRenderer.emitting = false;
            _rigidBody.velocity = Vector3.zero;
            Transform.position = _startPosition;
            //_pool.Release(this);
            _bulletPool.Take(this);
        }
    }
}