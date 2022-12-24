using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZombieRush
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField, Range(3, 8)] private int health = 5;
        [SerializeField, Range(0, 5)] private int shield = 0;
        [SerializeField, Range(0f, 10f)] private float _damageCooldown = 3f;

        [SerializeField] private MeshRenderer[] _meshes;
        [SerializeField, ColorUsage(true, true)] private Color hitColor = Color.white;

        [SerializeField] private RectTransform[] healthIndicator;

        private int currentHealth = 0;
        private int currentSheild = 0;

        private bool isDead = false;

        #region Properties
        public bool IsDead { get => isDead; private set => isDead = value; }
        public int Health { get => health; private set => health = value; }
        public int Shield { get => shield; private set => shield = value; }
        public int CurrentHealth { get => currentHealth; private set => currentHealth = value; }
        public int CurrentSheild { get => currentSheild; private set => currentSheild = value; }
        #endregion

        private float damageCheckTimer = 0f;

        internal event Action<int> OnTakeDamage;
        internal event Action OnPlayerDead;

        internal void Init()
        {
            isDead = false;
            CurrentHealth = Health;
            CurrentSheild = Shield;

            SetHealthUI();
        }

        internal void UpdateHealth()
        {
            damageCheckTimer += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollidedWithEnemy(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            //OnCollidedWithEnemy(other);
        }

        private void OnCollidedWithEnemy(Collider other)
        {
            if (other.CompareTag("Enemy") && damageCheckTimer >= _damageCooldown)
            {
                damageCheckTimer = 0f;
                //TakeDamage(1);
            }
        }

        internal void TakeDamage(int damage)
        {
            if(isDead) return;

            if(Shield == 0)
            {
                CurrentHealth -= damage;
            }
            else
            {
                CurrentSheild -= damage;
            }

            OnTakeDamage?.Invoke(damage);

            UpdateHealthUI();
            VisualHitUpdate();

            if(CurrentHealth <= 0)
            {
                isDead = true;
                OnPlayerDead?.Invoke();
                // Dead
                // Show game over message
                // Show retry button
                // Show menu button
            }
        }

        private void SetHealthUI()
        {
            for(int i = 0; i < CurrentHealth; i++)
            {
                healthIndicator[i].gameObject.SetActive(true);
            }
        }

        internal void UpdateHealthUI()
        {
            if (healthIndicator[CurrentHealth].gameObject.activeInHierarchy)
            {
                healthIndicator[CurrentHealth].DOScale(0f, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
                {
                    healthIndicator[CurrentHealth].gameObject.SetActive(false);
                });
            }
        }

        internal void VisualHitUpdate()
        {
            for(int i = 0; i < _meshes.Length; i++)
            {
                _meshes[i].material.DOColor(hitColor, 0.08f).SetEase(Ease.OutBack).SetLoops(4, LoopType.Yoyo);
            }
            
        }
    }
}