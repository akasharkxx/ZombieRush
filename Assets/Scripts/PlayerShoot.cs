using Cinemachine;
using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ZombieRush
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private CameraShake _cameraShake;

        [SerializeField, Range(0.1f, 100f)] private float _fireRate;
        [SerializeField, Range(1f, 20f)] private float _bulletSpeed;

        [SerializeField, Range(0.1f, 10f)] private float _cameraShakeIntensity;
        [SerializeField, Range(0f, 0.3f)] private float _cameraShakeTime;

        [SerializeField] private bool _isAutoShoot = false;

        private float _tempTimer = 0f;
        [SerializeField] private float _fireInterval = 0f;

        private PlayerInput input;

        private IObjectPool<Bullet> _bulletPool;

        private void Awake()
        {
            _bulletPool = new ObjectPool<Bullet>(CreateNewBullet, TakeBulletFromPool, ReturnBulletToPool);
            input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            _fireInterval = 1 / _fireRate;
        }

        private void Update()
        {
            _tempTimer += Time.deltaTime;

            bool isTime = _tempTimer > _fireInterval;

            if (isTime)
            {
                if (_isAutoShoot)
                {
                    ShootBullet();
                }
                else if(input.AimInput != Vector2.zero)
                {
                    ShootBullet();
                }
            }
        }

        private Bullet CreateNewBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity, _bulletHolder);
            bullet.SetPool(_bulletPool);
            return bullet;
        }

        private void TakeBulletFromPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void ReturnBulletToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        internal void ShootBullet()
        {
            _tempTimer = 0f;
            var bullet = _bulletPool.Get();
            bullet.Transform.forward = _shootPoint.forward;
            bullet.Init(_shootPoint.position, _bulletSpeed);
            _cameraShake.Shake(_cameraShakeIntensity, _cameraShakeTime);
        }

        internal void UpdateFireRate(float addPercentage)
        {
            _fireRate += _fireRate * addPercentage;
            _fireInterval = 1 / _fireRate;
        }

        private void OnValidate()
        {
            _fireInterval = 1 / _fireRate;
        }
    }
}