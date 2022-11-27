using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        internal static GameManager Instance;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        [SerializeField] internal Transform playerTransform;
    }
}

