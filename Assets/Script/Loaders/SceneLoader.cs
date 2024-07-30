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
            StartCoroutine(LoadFightSceneWithDelay());
        }

        private IEnumerator LoadFightSceneWithDelay()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("FightScene");
        }
    }
}
