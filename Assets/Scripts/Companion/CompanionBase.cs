using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

namespace ZombieRush
{
    public class CompanionBase : MonoBehaviour
    {
        public float _detectRange = 1.0f;
        public float _checkInterval = 1.0f;
        public float _attackInterval = 1.0f;
        public float _attackRadius = 1.0f;

        public float _maxDistanceFromPlayer = 5.0f;

        public int _attackDamage = 10;

        public LayerMask _enemyMask;

        public Transform meleePoint;
        public CompanionAnimation anim;

        private NavMeshAgent _agent;
        private Transform _nearbyEnemyTransform;
        private Enemy _nearbyEnemy;
        [SerializeField] private Transform _target;

        public bool isPlayerMoving = false;
        public bool isLockedOnEnemy = false;
        public bool isAttacking = false;

        private float checkEnemyTimer = 0.0f;

        private void Start()
        {
            anim.OnAttackStart += Animation_OnAttackStart;
            anim.OnAttacking += Animation_OnAttack;
            anim.OnAttackComplete += Animation_OnAttackComplete;

            _agent = GetComponent<NavMeshAgent>();
        }

        private void Animation_OnAttackComplete()
        {
            isAttacking = false;
            isLockedOnEnemy = false;
            _nearbyEnemyTransform = null;
        }

        private void Animation_OnAttackStart()
        {
            isAttacking = true;
        }

        private void Animation_OnAttack()
        {
            AttackEnemy();
        }

        private void Update()
        {
            checkEnemyTimer += Time.deltaTime;

            float inputSquare = Vector2.SqrMagnitude(GameManager.Instance.playerInput);
            isPlayerMoving = inputSquare > 0.05f;


            if (isPlayerMoving)
            {
                isLockedOnEnemy = false;
                isAttacking = false;
                anim.PlayWalkAnimation();
            }
            else if (isLockedOnEnemy && !isAttacking)
            {
                float distance = Vector3.Distance(transform.position, _nearbyEnemyTransform.position);
                //Debug.Log($"Here : {distance}");
                if (distance < 1.5f)
                {
                    Debug.Log("Trying to attack");
                    anim.AttackStart();
                }
            }
            else
            {
                CheckForEnemyNearby();
            }
        }

        private void FixedUpdate()
        {
            if(isPlayerMoving || _nearbyEnemyTransform == null || _nearbyEnemy.IsDead)
            {
                _agent.SetDestination(_target.position);
            }
            else if(isLockedOnEnemy && !isAttacking)
            {
                _agent.SetDestination(_nearbyEnemyTransform.position);
            }
        }

        private void CheckForEnemyNearby()
        {

            if(checkEnemyTimer < _checkInterval || isLockedOnEnemy || isAttacking)
            {
                return;
            }
            Debug.Log("Checking for enemy");

            Collider[] enemiesNearby = Physics.OverlapSphere(transform.position, _detectRange, _enemyMask);

            float closestDistance = float.MaxValue;
            int index = -1;

            for(int i = 0; i < enemiesNearby.Length; i++)
            {
                if (enemiesNearby[i].CompareTag("Enemy"))
                {
                    if (enemiesNearby[i].TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        if (enemy.IsDead) continue;

                        float distance = Vector3.Distance(transform.position, enemiesNearby[i].transform.position);

                        if(distance > _maxDistanceFromPlayer)
                        {
                            continue;
                        }

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            index = i;
                        }
                    }
                }
            }

            if(index != -1)
            {
                _nearbyEnemy = enemiesNearby[index].GetComponent<Enemy>();
                _nearbyEnemyTransform = _nearbyEnemy.transform;
                isLockedOnEnemy = true;
                return;
            }

            isLockedOnEnemy = false;
        }

        private void AttackEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(meleePoint.position, _attackRadius);
            Debug.Log("Trying to attack again");
            for(int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].CompareTag("Enemy"))
                {
                    if (enemies[i].TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        if (enemy.IsDead) { continue; }

                        Debug.Log($"Attacking enemy : {enemy.gameObject.name}");
                        enemy.TakeDamage(_attackDamage);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(meleePoint.position, _attackRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectRange);
        }
    }
}
