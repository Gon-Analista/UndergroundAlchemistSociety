using System;
using System.Collections.Generic;
using System.Data;
using Script.BodyParts;
using Script.Loaders;
using Script.Modifiers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Homun
{
    [Serializable]
    public class Homun : MonoBehaviour
    {
        public HomunStats Stats { get; set; }
        private List<HomunTemporalStats> TemporalStats { get; set; }
        private float dotTimer = 0f;
        
        // Body Parts for Legs, Core and Arms. By default they are empty. One body part per type.
        public BodyPart core;
        public BodyPart legs;
        public BodyPart arms;
        public List<BodyPart> accessories;

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
        
        public List<ModifierData> GetActiveStatusModifiers()
        {
            var modifiers = new List<ModifierData>();
            foreach (var stat in TemporalStats)
            {
                modifiers.Add(stat.Source);
            }

            return modifiers;
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
        
        
        public void Attack(Homun target)
        {
            // Get all the modifiers
            var (enemyModifiers, selfModifiers) = GetAllBodyPartsOnHitModifiers();
            // Apply the modifiers to the target
            foreach (var modifier in enemyModifiers)
            {
                target.ReceiveTemporalStatus(modifier);
            }
            
            // Apply the modifiers to the self
            foreach (var modifier in selfModifiers)
            {
                ReceiveTemporalStatus(modifier);
            }
            
            // Calculate the damage
            var damage = Stats.Attack;
            // Vary the damage by a random factor
            damage *= Random.Range(0.9f, 1.1f);
            
            
            // Check if the attack is critical
            if (Random.value < Stats.CriticalChance)
            {
                damage *= (Stats.CriticalDamage/100);
            }
            // Apply the damage to the target
            Debug.Log("Dealing " + damage + " damage to the target");
            target.ReceiveDamage(damage);
        }

        private (List<ModifierData>, List<ModifierData>) GetAllBodyPartsOnHitModifiers()
        {
            var enemyModifiers = new List<ModifierData>();
            enemyModifiers.AddRange(core.stats.EnemyInflictingModifiers);
            enemyModifiers.AddRange(legs.stats.EnemyInflictingModifiers);
            enemyModifiers.AddRange(arms.stats.EnemyInflictingModifiers);
            foreach (var accessory in accessories)
            {
                enemyModifiers.AddRange(accessory.stats.EnemyInflictingModifiers);
            }
       

            var selfModifiers = new List<ModifierData>();
            selfModifiers.AddRange(core.stats.SelfInflictingModifiers);
            selfModifiers.AddRange(legs.stats.SelfInflictingModifiers);
            selfModifiers.AddRange(arms.stats.SelfInflictingModifiers);
            foreach (var accessory in accessories)
            {
                selfModifiers.AddRange(accessory.stats.SelfInflictingModifiers);
            }
          
            
            return (enemyModifiers, selfModifiers);
        }

        public void ReceiveDamage(float damage)
        {
            // Calculate the damage reduction
            var damageReduction = Stats.DamageReduction;
            // Apply the damage reduction
            damage -= damage * (damageReduction / 100);
            // Apply the damage to the health
            Stats.Health -= damage;
            Debug.Log("Received " + damage + " damage. Current health: " + Stats.Health);
        }

        public void ReceiveTemporalStatus(ModifierData modifierData)
        {
            if (Random.value < modifierData.chance)
            {
                Debug.Log("Applying modifier: " + modifierData.modifier);
                TemporalStats.Add(new HomunTemporalStats(modifierData));
            }
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
        
        // MonoBehaviour methods
        private void Start()
        {
            Stats = new HomunStats();
            TemporalStats = new List<HomunTemporalStats>();
            
            UpdateStats();
            
            // Initialize the body parts GameObjects
            InitializeBodyPart(core, Vector2.zero);
            InitializeBodyPart(legs, Vector2.zero);
            InitializeBodyPart(arms, Vector2.zero);
            foreach (var accessory in accessories)
            {
                InitializeBodyPart(accessory, Vector2.zero);
            }
        }
        
    private void Update(){
        dotTimer += Time.deltaTime;

        if (dotTimer >= 1f)
        {
            ApplyDotEffects();
            dotTimer = 0f; 
        }

        // Actualizar la duración de los efectos temporales
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

    private void ApplyDotEffects()
    {
        float poisonDamage = 0f;
        float burnDamage = 0f;
        float bleedDamage = 0f;

        foreach (var stat in TemporalStats)
        {
            if (Time.time - stat.LastDotTime >= 1f)
            {
                switch (stat.DotType)
                {
                    case StatusModifier.Poison:
                        poisonDamage += stat.DotDamage;
                        break;
                    case StatusModifier.Burn:
                        burnDamage += stat.DotDamage;
                        break;
                    case StatusModifier.Bleed:
                        bleedDamage += stat.DotDamage;
                        break;
                }
                stat.LastDotTime = Time.time;
            }
        }

        if (poisonDamage > 0)
        {
            ReceiveDamage(poisonDamage);
            Debug.Log($"{gameObject.name} received {poisonDamage} poison damage");
        }
        if (burnDamage > 0)
        {
            ReceiveDamage(burnDamage);
            Debug.Log($"{gameObject.name} received {burnDamage} burn damage");
        }
        if (bleedDamage > 0)
        {
            ReceiveDamage(bleedDamage);
            Debug.Log($"{gameObject.name} received {bleedDamage} bleed damage");
        }
    }

        public void Clone(Homun homun)
        {
            Stats = homun.Stats.Clone();
            TemporalStats = new List<HomunTemporalStats>();
            foreach (var stat in homun.TemporalStats)
            {
                TemporalStats.Add(stat);
            }
            core = homun.core;
            legs = homun.legs;
            arms = homun.arms;
            accessories = new List<BodyPart>();
            foreach (var accessory in homun.accessories)
            {
                accessories.Add(accessory);
            }
        }
        
        public static Homun CreateRandomHomun(int difficulty, GameObject gameObject)
        {
            var homun = gameObject.AddComponent<Homun>();
            homun.Stats = new HomunStats();
            homun.TemporalStats = new List<HomunTemporalStats>();
            homun.accessories = new List<BodyPart>();
            
            // Get a random core
            homun.core = BodyPartManager.Instance.GetRandomBodyPartByType(BodyPartType.Core);
            homun.legs = null;
            homun.arms = null;
            homun.accessories.Clear();
            homun.isPlayer = false;

            if (difficulty == 0)
            {
                homun.core = BodyPartManager.Instance.GetPartById("core_weakling");
            }
            
            // Get a random leg
            if (difficulty >= 1)
            {
                homun.core = BodyPartManager.Instance.GetRandomBodyPartByType(BodyPartType.Core);
            }
            
            // Get a random arm
            if (difficulty >= 2)
            {
                homun.legs = BodyPartManager.Instance.GetRandomBodyPartByType(BodyPartType.Legs);
            }

            if (difficulty >= 3)
            {
                homun.arms = BodyPartManager.Instance.GetRandomBodyPartByType(BodyPartType.Arms);
            }
            
            // Get a random accessory
            if (difficulty >= 4)
            {
                homun.accessories.Add(BodyPartManager.Instance.GetRandomBodyPartByType(BodyPartType.Accessory));
            }
            
            homun.UpdateStats();
            return homun;
        }
        
        
    }
