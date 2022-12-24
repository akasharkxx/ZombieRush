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

        [SerializeField] internal ScoreHandler scoreHandler;

        internal Vector2 playerInput;

        internal void EnemyDied(int someId = 0)
        {

        }
    }
}

