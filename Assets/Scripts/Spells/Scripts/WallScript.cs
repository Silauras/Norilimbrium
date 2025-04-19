using UnityEngine;

namespace Spells.Scripts
{
    public class WallScript : MonoBehaviour
    {
        public float baseLifeTime = 0.2f;
        public bool growFlag = true;
        public bool shakeFlag = true;
        public bool shrinkFlag = true;

        public float growDuration = 1f;
        public float shakeIntensity = 0.01f;
        public float shakeDuration = 8.8f;
        public float shrinkAmount = 0.2f;

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
            float elapsedTime = 0;
            Vector3 startingScale = transform.localScale / 8f;

            while (elapsedTime < growDuration)
            {
                transform.localScale = Vector3.Lerp(startingScale, _originalScale, (elapsedTime / growDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = _originalScale;

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

            yield return new WaitForSeconds(baseLifeTime);
            Destroy(gameObject);
        }
    }
}