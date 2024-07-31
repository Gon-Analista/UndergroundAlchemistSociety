using System.Collections;
using TMPro;

namespace Script.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DamageTextManager : MonoBehaviour
    {
        public GameObject damageTextPrefab; // Assign this in the Inspector

        public void ShowDamage(int damageAmount, bool isCritic, bool isEnemy)
        {
            // Determine the appropriate damage zone
            GameObject playerDamageZone = GameObject.Find("PlayerDamageZone");
            GameObject enemyDamageZone = GameObject.Find("EnemyDamageZone");
            
            var damageZone = isEnemy ? enemyDamageZone : playerDamageZone;

            // Calculate a random position inside the damage zone
            //Vector3 randomPosition = GetRandomPositionInsideZone(damageZone.GetComponentInChildren<Rect>());

            // Set the damage text
            //TextMeshProUGUI damageText = damageTextInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (damageZone == null)
                return;

            var damageTextObject = Instantiate(damageTextPrefab, damageZone.transform);
            TextMeshProUGUI damageText = damageTextObject.GetComponentInChildren<TextMeshProUGUI>();
            if (damageText != null)
            {
                // Set the damage text and color
                damageText.text = damageAmount.ToString();
                damageText.color = !isCritic ? Color.yellow : Color.magenta;
                StartCoroutine(AnimateDamageText(damageText.gameObject, damageZone.transform.position));
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on the damage text prefab.");
            }
        }
        
        private Vector3 GetRandomPositionInsideZone(Rect damageZone)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(damageZone.xMin, damageZone.xMax),
                Random.Range(damageZone.yMin, damageZone.yMax),
                0
            );

            return randomPosition;
        }

        private IEnumerator AnimateDamageText(GameObject damageTextInstance, Vector3 initialPosition)
        {
            float duration = 0.5f; // Duration of the animation
            float elapsedTime = 0f;
    
            // define initialPosition as the center of the gameobject
            Vector3 targetPosition = initialPosition + new Vector3(0, 10, 0); // Move upwards

            CanvasGroup canvasGroup = damageTextInstance.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = damageTextInstance.AddComponent<CanvasGroup>();
            }

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Move the text upwards
                damageTextInstance.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

                // Fade out the text
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);

                yield return null;
            }

            // Destroy the text after the animation
            Destroy(damageTextInstance);
        }
    }
}