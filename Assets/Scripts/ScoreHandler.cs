using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    // Temp player prefs keys
    // TODO : use json or other data saving way
    private readonly string PLAYER_LIFETIME_SCORE = "Player_Lifetime_Score";
    private readonly string ENEMIES_LIFETIME_KILLED = "Enemies_Lifetime_Killed";
    private readonly string PLAYER_PROFILE_LEVEL = "Player_Profile_Level";
    
    [SerializeField] private int _playerLifeTimeScore = 0;
    [SerializeField] private int _playerCurrentGameScore = 0;

    [SerializeField] private int _enemiesLifeTimeKilled = 0;
    [SerializeField] private int _enemiesKilledInCurrentGame = 0;

    [SerializeField] private int _playerProfileLevel = 0;
    [SerializeField] private int _playerCurrentGameLevel = 0;

    public int PlayerLifeTimeScore { get => _playerLifeTimeScore; private set => _playerLifeTimeScore = value; }
    public int PlayerCurrentGameScore { get => _playerCurrentGameScore; private set => _playerCurrentGameScore = value; }
    public int EnemiesLifeTimeKilled { get => _enemiesLifeTimeKilled; private set => _enemiesLifeTimeKilled = value; }
    public int EnemiesKilledInCurrentGame { get => _enemiesKilledInCurrentGame; private set => _enemiesKilledInCurrentGame = value; }
    public int PlayerProfileLevel { get => _playerProfileLevel; private set => _playerProfileLevel = value; }
    public int PlayerCurrentGameLevel { get => _playerCurrentGameLevel; private set => _playerCurrentGameLevel = value; }

    internal void Init()
    {
        PlayerLifeTimeScore = PlayerPrefs.GetInt(PLAYER_LIFETIME_SCORE);
        PlayerCurrentGameScore = 0;
        PlayerProfileLevel = PlayerPrefs.GetInt(PLAYER_PROFILE_LEVEL);
        PlayerCurrentGameLevel = 0;
        EnemiesLifeTimeKilled = PlayerPrefs.GetInt(ENEMIES_LIFETIME_KILLED);
        EnemiesKilledInCurrentGame = 0;
    }

    internal int UpdateCurrentGameScore(int value)
    {
        PlayerCurrentGameScore += value;

        return PlayerCurrentGameScore;
    }

    internal int UpdateLifeTimeScore(int value = 0)
    {
        PlayerLifeTimeScore += PlayerCurrentGameScore;

        return PlayerLifeTimeScore;
    }

    internal int UpdateCurrentGameLevel(int value)
    {
        PlayerCurrentGameLevel += value;

        return PlayerCurrentGameLevel;
    }

    internal int UpdateProfileLevel(int value = 0)
    {
        PlayerProfileLevel += value;

        return PlayerProfileLevel;
    }

    internal int UpdateEnemyKilledInGame(int value)
    {
        EnemiesKilledInCurrentGame += value;
        
        return EnemiesKilledInCurrentGame;
    }

    internal int UpdateTotalEnemyKilled(int value)
    {
        EnemiesLifeTimeKilled += value;
        return EnemiesLifeTimeKilled;
    }

    internal void Save()
    {
        SaveToPlayerPrefs();
    }

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt(PLAYER_LIFETIME_SCORE, PlayerLifeTimeScore);
        PlayerPrefs.SetInt(ENEMIES_LIFETIME_KILLED, EnemiesLifeTimeKilled);
    }
}
