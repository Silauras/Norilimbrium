using System;
using UnityEngine;

namespace Code.Scripts.Characters.CharacterStatsComponents
{
    public class StaminaComponent : MonoBehaviour
    {
        public float maxStamina = 100f;
        public float currentStamina = 100f;

        public float timeBeforeStaminaReturns = 2f;
        public float staminaRegenerationRate = 1f;

       

        // ReSharper disable Unity.PerformanceAnalysis
        public void SubtractStamina(float value)
        {
            currentStamina -= value;
            if (currentStamina < maxStamina)
            {
                CancelInvoke(nameof(RegenerateStamina));
                InvokeRepeating(nameof(RegenerateStamina), timeBeforeStaminaReturns, 0.1f);
            }
            else
            {
                CancelInvoke(nameof(RegenerateStamina));
            }
        }

        private void RegenerateStamina()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenerationRate;    
            }
        }
    }
}