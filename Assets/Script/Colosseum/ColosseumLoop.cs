using System.Collections.Generic;
using Script.BodyParts;
using Script.Loaders;
using Script.UI;
using UnityEngine;

namespace Script.Colosseum
{
    public class ColosseumLoop : MonoBehaviour
    {
        public HomunFight currentFight;
        public List<HomunFight> nextFights;
        
        // Using GameManager's round value, calculate a difficulty. Every 5 rounds, it increases.
        
        // Using GameManager's round value, generate a list of bodyparts to be given as prize. They have to be 3, and
        // the round 0 is a leg, round 1 is arm, and from round 2 onwards is a random of any kind
        // They always have to be 3 body parts and they have to be unique to each other
        private List<BodyPart> GeneratePrize()
        {
            var parts = new List<BodyPart>();
            var round = GameManager.Instance.round;
            var partManager = BodyPartManager.Instance;
            
            // If GameManager.Instance.homun doesn't have legs, we should GetRandomSampleWithoutAccessory, else, withoutCore
            if (GameManager.Instance.homun.legs == null)
            {
                parts.AddRange(partManager.GetRandomSampleWithoutAccessories(3));
            }
            else
            {
                parts.AddRange(partManager.GetRandomSampleWithoutCore(3));
            }
            return parts;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            currentFight = gameObject.AddComponent<HomunFight>();
            var fightUI = GameObject.Find("HomunFightUI").GetComponent<FightUIManager>();
            fightUI.currentFight = currentFight;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentFight.result == FightResult.Won)
            {
                GameManager.Instance.SetPartPrizesAndShow(GeneratePrize());
            }
        }
    }
}
