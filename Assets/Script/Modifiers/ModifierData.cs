using System;

namespace Script.Modifiers
{
    [Serializable]
    public class ModifierData
    {
        public StatusModifier modifier;
        public float chance;
        public float duration;

        public ModifierData(StatusModifier modifier, float chance, float duration)
        {
            this.modifier = modifier;
            this.chance = chance;
            this.duration = duration;
        }
    }
}