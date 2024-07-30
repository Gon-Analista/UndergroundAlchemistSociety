using System.Collections.Generic;
using Script.BodyParts;
using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Script.Loaders
{
    public class SceneLoader : MonoBehaviour
    {
        public float delay = 1.0f; // Adjust the delay as needed

        public void LoadTitleScreen()
        {
            SceneManager.LoadScene("TitleScreen");
        }

        public void LoadFightScene()
        {
            SceneManager.LoadScene("FightScene");
        }

        private IEnumerator LoadSceneWithDelay()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("CoreSelectionScene");
        }

        public void LoadCoreSelection()
        {
            Debug.Log("Loading Core Selection Scene");

            var coreParts = new List<BodyPart>
            {
                BodyPartManager.Instance.GetPartById("core_ice_core"),
                BodyPartManager.Instance.GetPartById("core_fire_core"),
                BodyPartManager.Instance.GetPartById("core_lightning_core")
            };
            
            GameManager.Instance.SetCurrentPrizes(coreParts);
            StartCoroutine(LoadSceneWithDelay());
        }

        public void LoadBodyPartSelection(List<BodyPart> bodyParts)
        {
            GameManager.Instance.SetCurrentPrizes(bodyParts);
            SceneManager.LoadScene("CoreSelectionScene");
        }

        public void LoadEquipmentSetupScene()
        {
            SceneManager.LoadScene("EquipmentScene");
        }
    }
}
