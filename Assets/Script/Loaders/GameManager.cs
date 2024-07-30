using System;
using System.Collections.Generic;
using Script.BodyParts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Loaders
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public Homun.Homun homun;
        public List<BodyPart> bodyParts;
        public int round;
        public int fighterPoints;

        public List<BodyPart> currentPrizes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitializeOnLoad()
        {
            var bodyPartManager = Loaders.BodyPartManager.Instance;
        }
        
        public int CalculateDifficulty()
        {
            return GameManager.Instance.round / 5;
        }
        
        public void SetCurrentPrizes(List<BodyPart> prizes)
        {
            currentPrizes = prizes;
        }
        
        public List<BodyPart> GetBodyParts()
        {
            return bodyParts;
        }
        
        public void AddBodyPart(BodyPart bodyPart)
        {
            bodyParts.Add(bodyPart);
        }
        
        public void RemoveBodyPart(BodyPart bodyPart)
        {
            bodyParts.Remove(bodyPart);
        }
        
        public void AddFighterPoints(int points)
        {
            fighterPoints += points;
        }

        private void LoadInitialData()
        {
            bodyParts = new List<BodyPart>();
            fighterPoints = 0;
            round = 0;
            
            // Set Current prizes to core_ice_core, core_fire_core, core_lightning_core
            currentPrizes = new List<BodyPart>();
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadInitialData();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetPartPrizesAndShow(List<BodyPart> parts)
        {
            currentPrizes = parts;
            round += 1;
            SceneLoader sceneLoader = gameObject.AddComponent<SceneLoader>();
            sceneLoader.LoadBodyPartSelection(currentPrizes);
        }

        public void SelectDropAndContinue(string coreId)
        {
            if (round == 0)
            {
                SelectCoreAndStart(coreId);
            }
            else
            {
                bodyParts.Add(
                    BodyPartManager.Instance.GetPartById(coreId)
                );
                SceneLoader sceneLoader = gameObject.AddComponent<SceneLoader>();
                sceneLoader.LoadEquipmentSetupScene();
            }
        }
        
        public void SelectCoreAndStart(string coreId)
        {
            var core = BodyPartManager.Instance.GetPartById(coreId);
            if (homun == null)
            {
                homun = gameObject.AddComponent<Homun.Homun>();
                homun.isPlayer = true;
                homun.gameObject.GetComponent<Renderer>().enabled = false;
            }

            homun.EquipBodyPart(core);
            SceneLoader sceneLoader = gameObject.AddComponent<SceneLoader>();
            sceneLoader.LoadFightScene();
        }
    }
}