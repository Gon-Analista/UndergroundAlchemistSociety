using System.Collections.Generic;
using Script.BodyParts;
using UnityEngine;

namespace Script.Loaders
{
    public class BodyPartManager: MonoBehaviour
    {
        public static BodyPartManager Instance { get; private set; }
        public List<BodyPart> bodyPartDatabase;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadBodyParts();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadBodyParts()
        {
            // Load all BodyPart ScriptableObjects from the Resources/BodyParts folder
            bodyPartDatabase = new List<BodyPart>(Resources.LoadAll<BodyPart>("BodyParts"));

            // Optionally, log the loaded body parts for debugging
            foreach (var bodyPart in bodyPartDatabase)
            {
                Debug.Log($"Loaded BodyPart: {bodyPart.name} (ID: {bodyPart.id})");
            }
        }
        
        public BodyPart GetPartById(string id)
        {
            return bodyPartDatabase.Find(item => item.id == id);
        }
    
        // Get body parts by type
        public List<BodyPart> GetBodyPartsByType(BodyPartType type)
        {
            return bodyPartDatabase.FindAll(item => item.partType == type);
        }
        
        // Get a random body part by type
        public BodyPart GetRandomBodyPartByType(BodyPartType type)
        {
            // Exclude all parts that contain "weakling"
            var parts = GetBodyPartsByType(type);
            // Remove all parts that contain "weakling"
            parts.RemoveAll(part => part.name.Contains("weakling"));
            return parts[Random.Range(0, parts.Count)];
        }
        
        // Get a random sample by type, without repeating bodyparts.
        public List<BodyPart> GetRandomSampleByType(BodyPartType type, int count)
        {
            var parts = GetBodyPartsByType(type);
            var sample = new List<BodyPart>();
            for (int i = 0; i < count; i++)
            {
                var randomPart = parts[Random.Range(0, parts.Count)];
                if (!sample.Contains(randomPart))
                {
                    sample.Add(randomPart);
                }
                else
                {
                    i--;
                }
            }
            return sample;
        }
        
        // Get a random sample, without repeating the same bodyparts.
        public List<BodyPart> GetRandomSample(int count)
        {
            var sample = new List<BodyPart>();
            for (int i = 0; i < count; i++)
            {
                var randomPart = bodyPartDatabase[Random.Range(0, bodyPartDatabase.Count)];
                if (!sample.Contains(randomPart))
                {
                    sample.Add(randomPart);
                }
                else
                {
                    i--;
                }
            }
            return sample;
        }
    }
}