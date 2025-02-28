using UnityEngine;

namespace Spells
{
    public class WallScript : MonoBehaviour
    {
        public float baseLifeTime = 5f; // Базовое время до уничтожения объекта
        public bool growFlag = true; // Флаг для роста объекта
        public bool shakeFlag = true; // Флаг для тряски
        public bool shrinkFlag = true; // Флаг для уменьшения по высоте

        public float growDuration = 1f; // Время роста
        public float shakeIntensity = 0.1f; // Интенсивность тряски
        public float shakeDuration = 0.5f; // Длительность тряски
        public float shrinkAmount = 0.2f; // Насколько уменьшить высоту

        private Vector3 _originalScale;

        void Start()
        {
            _originalScale = transform.localScale;

            if (growFlag)
            {
                StartCoroutine(GrowAndShake());
            }
            else
            {
                StartCoroutine(GrowAndShake());
            }
        }

        private System.Collections.IEnumerator GrowAndShake()
        {
            // Рост объекта
            float elapsedTime = 0;
            Vector3 startingScale = transform.localScale / 8f;

            while (elapsedTime < growDuration)
            {
                transform.localScale = Vector3.Lerp(startingScale, _originalScale, (elapsedTime / growDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = _originalScale;

            // Тряска и уменьшение по высоте
            if (shakeFlag || shrinkFlag)
            {
                Vector3 originalPosition = transform.position;
                float shakeElapsedTime = 0;

                float startHeight = transform.localScale.y;
                while (shakeElapsedTime < shakeDuration)
                {
                    if (shrinkFlag)
                    {
                        Vector3 newScale = transform.localScale;
                        newScale.y = Mathf.Lerp(startHeight, startHeight - startHeight * shrinkAmount, (elapsedTime / shakeDuration));
                        transform.localScale = newScale;
                    }
                    if (shakeFlag)
                    {
                        Vector3 randomShake = originalPosition + Random.insideUnitSphere * shakeIntensity;
                        transform.position = new Vector3(randomShake.x, randomShake.y, originalPosition.z);
                        shakeElapsedTime += Time.deltaTime;
                    }

                    yield return null;
                }

                transform.position = originalPosition;
            }

            // Уничтожение объекта после базового времени
            yield return new WaitForSeconds(baseLifeTime);
            Destroy(gameObject);
        }
    }
}