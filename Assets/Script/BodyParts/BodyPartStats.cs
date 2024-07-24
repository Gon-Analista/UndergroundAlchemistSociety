using System;
using System.Collections.Generic;
using Script.Modifiers;
using UnityEngine;

namespace Script.BodyParts
{
    [Serializable]
    public class BodyPartStats : StatsModifier
    {
        [SerializeField] private List<ModifierData> selfInflictingModifiers;
        [SerializeField] private List<ModifierData> enemyInflictingModifiers;
    
        public List<ModifierData> SelfInflictingModifiers { get => selfInflictingModifiers; set => selfInflictingModifiers = value; }
        public List<ModifierData> EnemyInflictingModifiers { get => enemyInflictingModifiers; set => enemyInflictingModifiers = value; } 
        
        public BodyPartStats()
        {
            selfInflictingModifiers = new List<ModifierData>();
            enemyInflictingModifiers = new List<ModifierData>();
        }
    }
}