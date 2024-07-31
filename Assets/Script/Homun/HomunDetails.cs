using System;
using System.Collections.Generic;
using System.Data;
using Script.BodyParts;
using Script.Loaders;
using Script.Modifiers;
using Script.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Homun
{
    [Serializable]
    public class HomunDetails : MonoBehaviour
    {
        public HomunStats Stats { get; set; }
        public BodyPart core;
        public BodyPart legs;
        public BodyPart arms;
        public List<BodyPart> accessories = new ();
        public bool isPlayer;
        
        private void UpdateStats()
        {
            // Reset stats
            Stats = new HomunStats();

            Stats.AddBodyPartStats(core);
            Stats.AddBodyPartStats(legs);
            Stats.AddBodyPartStats(arms);
            foreach (var accessory in accessories)
            {
                Stats.AddBodyPartStats(accessory);
            }
        }

        public void EquipBodyPart(string bodyPartId)
        {
            var bodyPart = BodyPartManager.Instance.GetPartById(bodyPartId);
            AddBodyPart(bodyPart);
        }

        public void EquipBodyPart(BodyPart bodyPart)
        {
            AddBodyPart(bodyPart);
        }

        private void AddBodyPart(BodyPart part)
        {
            switch (part.partType)
            {
                case BodyPartType.Core:
                    core = part;
                    break;
                case BodyPartType.Legs:
                    legs = part;
                    break;
                case BodyPartType.Arms:
                    arms = part;
                    break;
                case BodyPartType.Accessory:
                    accessories.Add(part);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateStats();
        }

        // This function will add the temporal stat modifiers to the HomunStat and return it modified, without modifying the original one
        public HomunStats GetStats()
        {
            var modifiedStats = Stats.Clone();
            // Next, we take all the modifiedStats, and use a Math.max(0, value) to avoid negative numbers
            modifiedStats.Health = Math.Max(0, modifiedStats.Health);
            modifiedStats.Mana = Math.Max(0, modifiedStats.Mana);
            modifiedStats.DamageReduction = Math.Max(0, modifiedStats.DamageReduction);
            modifiedStats.Attack = Math.Max(0, modifiedStats.Attack);
            modifiedStats.Speed = Math.Max(0, modifiedStats.Speed);
            modifiedStats.AbilityPower = Math.Max(0, modifiedStats.AbilityPower);
            modifiedStats.Evasion = Math.Max(0, modifiedStats.Evasion);
            modifiedStats.CriticalChance = Math.Max(0, modifiedStats.CriticalChance);
            modifiedStats.CriticalDamage = Math.Max(0, modifiedStats.CriticalDamage);
            modifiedStats.Accuracy = Math.Max(0, modifiedStats.Accuracy);

            return modifiedStats;
        }


        private (List<ModifierData>, List<ModifierData>) GetAllBodyPartsOnHitModifiers()
        {
            var enemyModifiers = new List<ModifierData>();
            var selfModifiers = new List<ModifierData>();
            enemyModifiers.AddRange(core.stats.EnemyInflictingModifiers);
            selfModifiers.AddRange(core.stats.SelfInflictingModifiers);
            if (legs != null)
            {
                enemyModifiers.AddRange(legs.stats.EnemyInflictingModifiers);
                selfModifiers.AddRange(legs.stats.SelfInflictingModifiers);
            }
            if (arms != null)
            {
                enemyModifiers.AddRange(arms.stats.EnemyInflictingModifiers);
                selfModifiers.AddRange(arms.stats.SelfInflictingModifiers);
            }
           if (accessories != null)
            {
                foreach (var accessory in accessories)
                {
                    enemyModifiers.AddRange(accessory.stats.EnemyInflictingModifiers);
                    selfModifiers.AddRange(accessory.stats.SelfInflictingModifiers);
                
                }
            }

            return (enemyModifiers, selfModifiers);
        }

        private GameObject InitializeBodyPart(BodyPart bodyPart, Vector2 localPosition)
        {
            if (bodyPart != null)
            {
                GameObject partObject = new GameObject(bodyPart.name);
                partObject.transform.parent = transform; // Set as child of Homun
                partObject.transform.localPosition = localPosition;

                SpriteRenderer spriteRenderer = partObject.AddComponent<SpriteRenderer>();
                BodyPartDisplay bodyPartDisplay = partObject.AddComponent<BodyPartDisplay>();
                bodyPartDisplay.SetBodyPart(bodyPart, isPlayer);

                return partObject;
            }

            return null;
        }
    }
}
