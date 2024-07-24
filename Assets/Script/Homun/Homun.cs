using System;
using System.Collections.Generic;
using System.Data;
using Script.BodyParts;
using Script.Loaders;
using Script.Modifiers;
using UnityEngine;

namespace Script.Homun
{
    public class Homun : MonoBehaviour
    {
        private HomunStats Stats { get; set; }
        private List<HomunTemporalStats> TemporalStats { get; set; }
        
        // Body Parts for Legs, Core and Arms. By default they are empty. One body part per type.
        private BodyPart Core { get; set; }
        private BodyPart Legs { get; set; }
        private BodyPart Arms { get; set; }

        private void UpdateStats()
        {
            // Reset stats
            Stats = new HomunStats();
            
            Stats.AddBodyPartStats(Core);
            Stats.AddBodyPartStats(Legs);
            Stats.AddBodyPartStats(Arms);
        }

        public void EquipBodyPart(int bodyPartId)
        {
            var bodyPart = BodyPartManager.Instance.GetPartById(bodyPartId);
            AddBodyPart(bodyPart);
        }
        
        private void AddBodyPart(BodyPart part)
        {
            switch (part.partType)
            {
                case BodyPartType.Core:
                    Core = part;
                    break;
                case BodyPartType.Legs:
                    Legs = part;
                    break;
                case BodyPartType.Arms:
                    Arms = part;
                    break;
                default:
                    break;
            }
            UpdateStats();
        }
        
        // This function will add the temporal stat modifiers to the HomunStat and return it modified, without modifying the original one
        public HomunStats GetModifiedStats()
        {
            var modifiedStats = Stats.Clone();
            foreach (var stat in TemporalStats)
            {
                // Add the stats from the temporal stat incStat, including the percentual add
                // Remember that the attributes are named incStat, incPercentStat, etc.
                modifiedStats.Health += stat.incHealth + (modifiedStats.Health * stat.incPercentHealth);
                modifiedStats.Mana += stat.incMana + (modifiedStats.Mana * stat.incPercentMana);
                modifiedStats.DamageReduction += stat.incDamageReduction + (modifiedStats.DamageReduction * stat.incPercentDamageReduction);
                modifiedStats.Attack += stat.incAttack + (modifiedStats.Attack * stat.incPercentAttack);
                modifiedStats.Speed += stat.incSpeed + (modifiedStats.Speed * stat.incPercentSpeed);
                modifiedStats.AbilityPower += stat.incAbilityPower + (modifiedStats.AbilityPower * stat.incPercentAbilityPower);
                modifiedStats.Evasion += stat.incEvasion + (modifiedStats.Evasion * stat.incPercentEvasion);
                modifiedStats.CriticalChance += stat.incCriticalChance + (modifiedStats.CriticalChance * stat.incPercentCriticalChance);
                modifiedStats.CriticalDamage += stat.incCriticalDamage + (modifiedStats.CriticalDamage * stat.incPercentCriticalDamage);
                modifiedStats.Accuracy += stat.incAccuracy + (modifiedStats.Accuracy * stat.incPercentAccuracy);
            }

            return modifiedStats;
        }
        
        
        // MonoBehaviour methods
        private void Start()
        {
            Stats = new HomunStats();
            TemporalStats = new List<HomunTemporalStats>();
        }
        
        private void Update()
        {
            // Check for temporal stats, and remove the ones that are over from the list.
            for (var i = TemporalStats.Count - 1; i >= 0; i--)
            {
                if (TemporalStats[i].Duration <= 0)
                {
                    TemporalStats.RemoveAt(i);
                }
                else
                {
                    TemporalStats[i].Duration -= Time.deltaTime;
                }
            }

        }
    }
}