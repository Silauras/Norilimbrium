using System;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;

    public static GameStateManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new GameStateManager();
        }

        return _instance;
    }

    private GameStateManager()
    {
    }

    [SerializeField] public GameState currentGameState = GameState.Game;

    /**
     * Representation of time in minutes
     * 1 hours contains 60 Minutes
     * 1 day contains 24 hours (1440 Minutes)
     * 1 week contains 7 days (10080 Minutes)
     * 13 weeks contains (131040 Minutes)
     */
    [SerializeField]
    public int Time { get; private set; } = 0;

    [SerializeField] [Range(min: .1f, max: 10f)]
    private float realSecondsForMinute = 3f;

    private float _gameTimer = 0f;


    private void FixedUpdate()
    {
        CountTime();
    }

    private void CountTime()
    {
        if (!currentGameState.Equals(GameState.Game)) return;
        _gameTimer += UnityEngine.Time.deltaTime;
        if (!(_gameTimer > realSecondsForMinute)) return;
        Time++;
        _gameTimer = 0;
    }
}