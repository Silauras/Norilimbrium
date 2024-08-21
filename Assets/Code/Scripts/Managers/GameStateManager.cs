using System;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    [SerializeField] private GameObject _inGameGUI;

    [SerializeField] private GameObject _backgroundGUI;
    
    [SerializeField] private GameObject _inventoryGUI;

    [SerializeField] private GameObject _pauseGUI;

    
    
    [SerializeField] public GameState currentGameState = GameState.Game;

    [SerializeField] public float defaultTimeScale = 1;
    [SerializeField] public float currentTimeScale = 1;

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

    private Material backGroundMaterial;
    private float _gameTimer = 0f;

    private void Start()
    {
        UpdateGUI();
        UpdateCursorState();
        backGroundMaterial = _backgroundGUI.GetComponent<RawImage>().material;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            currentGameState = currentGameState == GameState.Inventory ? GameState.Game : GameState.Inventory;
            UpdateGUI();
            UpdateCursorState();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            currentGameState = currentGameState is (GameState.Game or GameState.Dialogue) ? 
                GameState.PauseMenu : GameState.Game;
            UpdateGUI();
            UpdateCursorState();
        }

        
        backGroundMaterial.SetFloat("_unscaledTime", UnityEngine.Time.unscaledTime);
    }

    private void FixedUpdate()
    {
        CountTime();
    }

    private void CountTime()
    {
        if (currentGameState is not GameState.Game) return;
        _gameTimer += UnityEngine.Time.fixedDeltaTime;
        if (!(_gameTimer > realSecondsForMinute)) return;
        Time++;
        _gameTimer = 0;
    }

    private void UpdateCursorState()
    {
        if (currentGameState == GameState.Game)
        {
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;   
        }
        else
        {
            // Lock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void UpdateGUI()
    {
         _backgroundGUI.SetActive(currentGameState is not (GameState.Game or GameState.Dialogue));

        switch (currentGameState)
        {
            case GameState.Game:
                _inGameGUI.SetActive(true);
                _inventoryGUI.SetActive(false);
                UnityEngine.Time.timeScale = currentTimeScale;
                break;
            case GameState.PauseMenu:
                UnityEngine.Time.timeScale = 0;
                break;
            case GameState.CharacterMenu:
                break;
            case GameState.Dialogue:
                break;
            case GameState.Inventory:
                _inventoryGUI.SetActive(true);
                _inGameGUI.SetActive(false);
                UnityEngine.Time.timeScale = 0;
                break;
            case GameState.Journal:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}