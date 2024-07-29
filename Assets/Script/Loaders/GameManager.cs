using System;
using System.Collections.Generic;
using Script.BodyParts;
using UnityEngine;

namespace Script.Loaders
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public Homun.Homun homun;
        public List<BodyPart> bodyParts;
        public int fighterPoints;

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
            homun = gameObject.AddComponent<Homun.Homun>();
            bodyParts = new List<BodyPart>();
            fighterPoints = 0;
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}