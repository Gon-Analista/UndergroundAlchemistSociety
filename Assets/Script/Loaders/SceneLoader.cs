using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Loaders
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadTitleScreen()
        {
            SceneManager.LoadScene("TitleScreen");
        }
        
        public void LoadFightScene()
        {
            SceneManager.LoadScene("FightScene");
        }
    }
}