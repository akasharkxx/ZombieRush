using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public class CompanionAnimation : MonoBehaviour
    {
        private Animator anim;

        public event System.Action OnAttackStart;
        public event System.Action OnAttacking;
        public event System.Action OnAttackComplete;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        internal void PlayAnimation(string clipName)
        {
            anim.Play(clipName);
        }

        public void AttackStart()
        {
            OnAttackStart?.Invoke();
            Debug.Log("attack start");
            PlayAnimation("Attack");
        }

        internal void PlayWalkAnimation()
        {
            PlayAnimation("Walk");
        }

        public void Attack()
        {
            Debug.Log("Attacking");
            OnAttacking?.Invoke();
        }

        public void AttackDone()
        {
            PlayAnimation("Idle");
            Debug.Log("Attack done");
            OnAttackComplete?.Invoke();
        }
    }
}

