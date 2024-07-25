namespace Script.Modifiers
{
    public static class StatusModifierExtensions
    {
        public static bool IsDot(this StatusModifier modifier)
        {
            switch (modifier)
            {
                case StatusModifier.Poison:
                case StatusModifier.Burn:
                case StatusModifier.Bleed:
                    return true;
                default:
                    return false;
            }
        }
    }
}