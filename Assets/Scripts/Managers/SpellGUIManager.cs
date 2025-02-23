using Spells;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class SpellGUIManager : MonoBehaviour
    {

        public static SpellGUIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate SpellGUIManager detected! Destroying the new one.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        [SerializeField] private GameObject spellGUI;

        // [SerializeField] private GameObject rotationWheelSpeedGUIElement;
        // [SerializeField] private GameObject rhythmClickerGUIElement;
        [SerializeField] private GameObject holdReleaseGUIElement;

        public bool isSpellingNow = false;
        public SpellTypeInput CurrentSpellInput;

        [Header("Hold Release Input")]
        public bool isHold = true;

        public float holdTime = 2f;
        public float acceptableTime = 0.5f;
        public float currentTime = 0f;

        [SerializeField] private RectTransform level;
        [SerializeField] private RectTransform allTime;
        [SerializeField] private RectTransform activeTime;


        private void Update()
        {
            switch (CurrentSpellInput)
            {
                case null:
                    level.localPosition = Vector2.zero;
                    break;
                case HoldReleaseInput:
                    var levelPosition = Vector2.zero;
                    levelPosition.y = currentTime / holdTime * activeTime.sizeDelta.y - 75;
                    Debug.Log(currentTime / holdTime * activeTime.sizeDelta.y);
                    level.localPosition = levelPosition;
                    break;
            }

        }

        public void SetSpellTypeInput(SpellTypeInput input)
        {
            CurrentSpellInput = input;
            switch (CurrentSpellInput)
            {
                case null:
                    // rotationWheelSpeedGUIElement.SetActive(false);
                    // rhythmClickerGUIElement.SetActive(false);

                    // holdReleaseGUIElement.SetActive(false);
                    break;
                case HoldReleaseInput:
                    // rotationWheelSpeedGUIElement.SetActive(false);
                    // rhythmClickerGUIElement.SetActive(false);

                    // holdReleaseGUIElement.SetActive(true);
                    break;
            }
        }

    }
}