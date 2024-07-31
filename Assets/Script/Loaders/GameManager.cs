using System;
using System.Collections.Generic;
using Script.BodyParts;
using Script.Homun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Loaders
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public Homun.HomunDetails homun;
        public List<BodyPart> bodyParts;
        public int round;
        public int fighterPoints;

        public List<BodyPart> currentPrizes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitializeOnLoad()
        {
            var bodyPartManager = Loaders.BodyPartManager.Instance;
        }
        
        // Destroy and recreate instnace
        public void Reset()
        {
            round = 0;
            Destroy(homun);
            homun = gameObject.AddComponent<HomunDetails>();
        }
        
        public int CalculateDifficulty()
        {
            return round;
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
            Debug.LogError("SelectDropAndContinue");
            if (round == 0)
            {
                SelectCoreAndStart(coreId);
            }
            else
            {
                homun.EquipBodyPart(
                    BodyPartManager.Instance.GetPartById(coreId)
                );
                SceneLoader sceneLoader = gameObject.AddComponent<SceneLoader>();
                sceneLoader.LoadFightScene();
            }
            ShowHomun(false);
        }
        
        public void SelectCoreAndStart(string coreId)
        {
            Debug.LogError("SelectCoreAndStart");
            var core = BodyPartManager.Instance.GetPartById(coreId);
            if (homun == null)
            {
                Debug.LogError("DefaultHomunCreated");
                homun = gameObject.AddComponent<Homun.HomunDetails>();
                homun.isPlayer = true;
            }

            Debug.LogError("EquippedPart");
            homun.EquipBodyPart(core);
            SceneLoader sceneLoader = gameObject.AddComponent<SceneLoader>();
            sceneLoader.LoadFightScene();
        }
        
        public void ShowHomun(bool status)
        {
            // check if homun is defined, and if so, hide its rendering
            if (homun != null)
            {
                var render = GetComponent<SpriteRenderer>();
                if (render != null)
                {
                    render.enabled = status;
                }
            }
        }
    }
}