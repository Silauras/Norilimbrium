using Code.Scripts.Characters.CharacterStatsComponents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Scripts
{
    public class CharacterStatsManagerUIScript : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private Image healthBar;

        [SerializeField] private Image health;

        [SerializeField] private Image staminaBar;
        [SerializeField] private Image stamina;

        [SerializeField] private Image manaBar;
        [SerializeField] private Image mana;

        [SerializeField] private Image resonanceBar;
        [SerializeField] private Image resonance;


        [Header("Character Components")] [SerializeField]
        private HealthComponent healthComponent;

        [SerializeField] private StaminaComponent staminaComponent;
        [SerializeField] private ManaComponent manaComponent;
        [SerializeField] private ResonanceComponent resonanceComponent;

        [Header("Character Stats")] [SerializeField]
        private float maxHealth = 100f;

        [SerializeField] private float currentHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float currentStamina = 100f;
        [SerializeField] private float maxMana = 100f;
        [SerializeField] private float currentMana = 100f;
        [SerializeField] private float maxResonance = 100f;
        [SerializeField] private float currentResonance = 100f;


        [Header("Bar size")] [SerializeField] private float maxSize = 150;
        [SerializeField] private float minSize = 20;


        void Start()
        {
            UpdateAllBars();
            SetHealth(healthComponent.currentHeath);
            SetMaxHealth(healthComponent.maxHeath);
            
            SetMaxStamina(staminaComponent.maxStamina);
            SetStamina(staminaComponent.currentStamina);
            
            SetMaxMana(manaComponent.maxMana);
            SetMana(manaComponent.currentMana);
            
            SetMaxResonance(resonanceComponent.maxResonance);
            SetResonance(resonanceComponent.currentResonance);
        }

        void Update()
        {
            SetHealth(healthComponent.currentHeath);
            SetMaxHealth(healthComponent.maxHeath);
            
            SetMaxStamina(staminaComponent.maxStamina);
            SetStamina(staminaComponent.currentStamina);
            
            SetMaxMana(manaComponent.maxMana);
            SetMana(manaComponent.currentMana);
            
            SetMaxResonance(resonanceComponent.maxResonance);
            SetResonance(resonanceComponent.currentResonance);
            
        }

        public void SetHealth(float health)
        {
            if (Mathf.Approximately(currentHealth, health))
            {
               return; 
            }
            currentHealth = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthBar();
        }

        public void SetMaxHealth(float maxHealthValue)
        {
            if (Mathf.Approximately(maxHealth, maxHealthValue))
            {
                return;
            }

            maxHealth = maxHealthValue;
            ClampBarSize(healthBar, maxHealth);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthBar();
        }

        public void SetStamina(float stamina)
        {
            if (Mathf.Approximately(currentStamina, stamina))
            {
                return;
            }
            currentStamina = Mathf.Clamp(stamina, 0, maxStamina);
            UpdateStaminaBar();
        }

        public void SetMaxStamina(float maxStaminaValue)
        {
            if (Mathf.Approximately(maxStamina, maxStaminaValue))
            {
                return;
            }
            maxStamina = maxStaminaValue;
            ClampBarSize(staminaBar, maxStamina);
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateStaminaBar();
        }

        public void SetMana(float mana)
        {
            if (Mathf.Approximately(currentMana, mana))
            {
                return;
            }
            currentMana = Mathf.Clamp(mana, 0, maxMana);
            UpdateManaBar();
        }

        public void SetMaxMana(float maxManaValue)
        {
            if(Mathf.Approximately(maxMana, maxManaValue))
            {
                return;
            }
            maxMana = maxManaValue;
            ClampBarSize(manaBar, maxMana);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            UpdateManaBar();
        }

        public void SetResonance(float resonance)
        {
            if (Mathf.Approximately(currentResonance, resonance))
            {
                return;
            }
            currentResonance = Mathf.Clamp(resonance, 0, maxResonance);
            UpdateResonanceBar();
        }

        public void SetMaxResonance(float maxResonanceValue)
        {
            if (Mathf.Approximately(maxResonance, maxResonanceValue))
            {
                return;
            }
            maxResonance = maxResonanceValue;
            ClampBarSize(resonanceBar, maxResonance);
            currentResonance = Mathf.Clamp(currentResonance, 0, maxResonance);
            UpdateResonanceBar();
        }

        private void UpdateHealthBar()
        {
            float healthPercent = currentHealth / maxHealth;
            health.rectTransform.sizeDelta = new Vector2((healthBar.rectTransform.sizeDelta.x - 4) * healthPercent,
                health.rectTransform.sizeDelta.y);
        }

        private void UpdateStaminaBar()
        {
            float staminaPercent = currentStamina / maxStamina;
            stamina.rectTransform.sizeDelta = new Vector2((staminaBar.rectTransform.sizeDelta.x - 4) * staminaPercent,
                stamina.rectTransform.sizeDelta.y);
        }

        private void UpdateManaBar()
        {
            float manaPercent = currentMana / maxMana;
            mana.rectTransform.sizeDelta = new Vector2((manaBar.rectTransform.sizeDelta.x - 4) * manaPercent,
                mana.rectTransform.sizeDelta.y);
        }

        private void UpdateResonanceBar()
        {
            float resonancePercent = currentResonance / maxResonance;
            resonance.rectTransform.sizeDelta =
                new Vector2((resonanceBar.rectTransform.sizeDelta.x - 4) * resonancePercent,
                    resonance.rectTransform.sizeDelta.y);
        }


        private void ClampBarSize(Image image, float value)
        {
            image.rectTransform.sizeDelta =
                new Vector2(Mathf.Clamp(value, minSize, maxSize), image.rectTransform.sizeDelta.y);
        }

        private void UpdateAllBars()
        {
            UpdateHealthBar();
            UpdateStaminaBar();
            UpdateManaBar();
            UpdateResonanceBar();
        }

        // Этот метод вызовется в редакторе при изменении значений
        private void OnValidate()
        {
            ClampBarSize(healthBar, maxHealth);
            ClampBarSize(staminaBar, maxStamina);
            ClampBarSize(manaBar, maxMana);
            ClampBarSize(resonanceBar, maxResonance);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            currentResonance = Mathf.Clamp(currentResonance, 0, maxResonance);
            UpdateAllBars();
        }
    }
}