using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] internal PlayerMovement mPlayerMovement;
        [SerializeField] internal PlayerShoot mPlayerShoot;
        [SerializeField] internal PlayerHealth mPlayerHealth;
        [SerializeField] internal PlayerInput mPlayerInput;
        [SerializeField] internal CameraShake mCameraShake;

        // TODO: make another component
        [SerializeField] internal RectTransform gameOver;

        private bool isPlayerDead = false;

        private void Awake()
        {
            mPlayerShoot.AwakeInit();
        }

        private void Start()
        {
            isPlayerDead = false;

            mPlayerMovement.Init();
            mPlayerShoot.Init();
            mPlayerHealth.Init();

            mPlayerHealth.OnPlayerDead += MPlayerHealth_OnPlayerDead;
        }

        private void MPlayerHealth_OnPlayerDead()
        {
            isPlayerDead = true;

            gameOver.gameObject.SetActive(true);
            gameOver.localScale = Vector3.zero;
            gameOver.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
        }

        private void Update()
        {
            if (isPlayerDead) { return; }

            mPlayerInput.UpdateInput();
            mPlayerMovement.UpdateMovement();
            mPlayerShoot.UpdateShoot();
            mPlayerHealth.UpdateHealth();
        }

        private void FixedUpdate()
        {
            if (isPlayerDead) { return; }

            mPlayerMovement.UpdateFixedMovement();
        }
    }
}