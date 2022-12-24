using UnityEngine;
using UnityEngine.Pool;
using Redcode.Pools;

namespace ZombieRush
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private Transform[] _shootPoint;
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Bullet _bulletPrefab;

        [SerializeField, Range(0.1f, 100f)] private float _fireRate;
        [SerializeField, Range(1f, 20f)] private float _bulletSpeed;
        [SerializeField, Range(0.1f, 10f)] private float _cameraShakeIntensity;
        [SerializeField, Range(0f, 0.3f)] private float _cameraShakeTime;

        [SerializeField, Range(10, 300)] private int _pooledBulletCount;

        [SerializeField] private bool _enableCameraShake = true;
        [SerializeField] private bool _isAutoShoot = false;
        [SerializeField] private float _fireInterval = 0f;
        private float _tempTimer = 0f;

        private PlayerController _playerController;
        private Pool<Bullet> _pool;

        internal void AwakeInit()
        {
            _pool = Pool.Create(_bulletPrefab, _pooledBulletCount, _bulletHolder);
            _playerController = GetComponent<PlayerController>();
        }

        internal void Init()
        {
            _fireInterval = 1 / _fireRate;
        }

        internal void UpdateShoot()
        {
            _tempTimer += Time.deltaTime;

            bool isTime = _tempTimer > _fireInterval;

            if (isTime)
            {
                if (_isAutoShoot)
                {
                    ShootBullet();
                }
                else if(_playerController.mPlayerInput.AimInput != Vector2.zero || Input.GetButton("Fire1"))
                {
                    ShootBullet();
                }
            }
        }

        internal void ShootBullet()
        {
            _tempTimer = 0f;

            for(int i = 0; i < _shootPoint.Length; i++)
            {
                var bullet = _pool.Get();
                bullet.SetPool(_pool);
                bullet.Transform.forward = _shootPoint[i].forward;
                bullet.Init(_shootPoint[i].position, _bulletSpeed);
            }

            if (_enableCameraShake)
            {
                _playerController.mCameraShake.Shake(_cameraShakeIntensity, _cameraShakeTime);
            }
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