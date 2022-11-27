using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public class Zombie : Enemy
    {
        [SerializeField] private ParticleSystem _deadHitEffect;
        [SerializeField] private Collider _collider;

        internal override void Init(Vector3 startPosition, EnemyType type = EnemyType.Zombie)
        {
            base.Init(startPosition, type);

            _collider.enabled = true;

            OnDied += Zombie_OnDied;
            OnTakeDamage += Zombie_OnTakeDamage;
        }

        private void Zombie_OnDied()
        {
            _model.gameObject.SetActive(false);
            _collider.enabled = false;
            _deadHitEffect.Play();

            DOVirtual.DelayedCall(1f, DeadCleanup);
        }

        private void Zombie_OnTakeDamage(int value)
        {
            
        }
    }
}
