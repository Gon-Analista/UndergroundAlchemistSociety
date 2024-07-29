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
            }
            else
            {
                Destroy(gameObject);
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
    }
}