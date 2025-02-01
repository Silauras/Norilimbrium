using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Code.Scripts.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        #region Singleton

        public static GameStateManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of GameStateManager found!");
                return;
            }

            Instance = this;
        }

        #endregion

        [SerializeField] private GameObject inGameGUI;
        [SerializeField] private GameObject backgroundGUI;
        [SerializeField] private GameObject inventoryGUI;
        [SerializeField] private GameObject pauseGUI;


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
        public int Time { get; private set; }

        [SerializeField] [Range(min: .1f, max: 10f)]
        private float realSecondsForMinute = 3f;

        private Material _backGroundMaterial;
        private float _gameTimer;
        private static readonly int UnscaledTime = Shader.PropertyToID("_unscaledTime");

        private void Start()
        {
            // UpdateGUI();
            UpdateCursorState();
            _backGroundMaterial = backgroundGUI.GetComponent<RawImage>().material;
        }

        /*private void Update()
        {
            if (InputSystem.actions.FindAction("Inventory").WasPerformedThisFrame())
            {
                currentGameState = currentGameState == GameState.Inventory ? GameState.Game : GameState.Inventory;
                UpdateGUI();
                UpdateCursorState();
            }
            else if (InputSystem.actions.FindAction("Pause").WasPerformedThisFrame())
            {
                currentGameState = currentGameState is (GameState.Game or GameState.Dialogue)
                    ? GameState.PauseMenu
                    : GameState.Game;
                UpdateGUI();
                UpdateCursorState();
            }

            if (_backGroundMaterial)
            {
                _backGroundMaterial.SetFloat(UnscaledTime, UnityEngine.Time.unscaledTime);   
            }
        }

        private void FixedUpdate()
        {
            CountTime();
        }
        */

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
                Cursor.lockState = CursorLockMode.Confined;
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
            backgroundGUI.SetActive(currentGameState is not (GameState.Game or GameState.Dialogue));

            switch (currentGameState)
            {
                case GameState.Game:
                    inGameGUI.SetActive(true);
                    inventoryGUI.SetActive(false);
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
                    inventoryGUI.SetActive(true);
                    inGameGUI.SetActive(false);
                    UnityEngine.Time.timeScale = 0;
                    break;
                case GameState.Journal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}