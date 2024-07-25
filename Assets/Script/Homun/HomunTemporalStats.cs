using System;
using Script.Modifiers;

namespace Script.Homun
{
    public class HomunTemporalStats : StatsModifier
    {
        // Same as HomunStats, but with a few extra properties, such as the duration of the stats
        public ModifierData Source;
        public float Duration;

        public HomunTemporalStats(ModifierData modifierData)
        {
            Source = modifierData;
            
            // switch with the StatusModifier enum
            switch (modifierData.modifier)
            {
                case StatusModifier.Slow:
                    // Temporal stat with incPercentSpeed -50%
                    incPercentSpeed = -0.5f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Stun:
                case StatusModifier.Freeze:
                case StatusModifier.Paralyze:
                case StatusModifier.Sleep:
                    incSpeed = -10000;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Silence:
                    break;
                case StatusModifier.Blind:
                case StatusModifier.Disarm:
                    // Decrease accuracy by 50%
                    incPercentAccuracy = -0.5f;
                    Duration = modifierData.duration;
                    break;
                // TODO: DoTs
                case StatusModifier.Poison:
                case StatusModifier.Burn:
                case StatusModifier.Bleed:
                    break;
                case StatusModifier.Taunt:
                    // Increases Attack a bit, but decreases its Defense
                    incAttack = 5;
                    incDamageReduction = -10;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Weakness:
                    // Decreases Attack by 50%
                    incPercentAttack = -0.5f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Vulnerability:
                    // Decreases Defense by 50%
                    incPercentDamageReduction = -0.5f;
                    Duration = modifierData.duration;
                    break;
                case StatusModifier.Fear:
                    incPercentAccuracy = -0.5f;
                    Duration = modifierData.duration;
                    break;
                default:
                    break;
            }
        }
    }
}