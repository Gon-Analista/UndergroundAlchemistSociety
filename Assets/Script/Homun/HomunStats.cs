using Script.BodyParts;

namespace Script.Homun
{
    public class HomunStats
    {
        public float Health { get; set; }
        public float Mana { get; set; }
        
        public float DamageReduction { get; set; }
        public float Attack { get; set; }
        public float Speed { get; set; }
        public float AbilityPower { get; set; }
        public float Evasion { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
        public float Accuracy { get; set; }
        
        public HomunStats()
        {
            Health = 100;
            Mana = 100;
            DamageReduction = 0;
            Attack = 10;
            Speed = 0.1f;
            AbilityPower = 10;
            Evasion = 0;
            CriticalChance = 5;
            CriticalDamage = 25;
            Accuracy = 100;
        }
        
        // Function that takes a BodyPart, uses its BodyPartStats and adds it to the HomunStats
        public void AddBodyPartStats(BodyPart bodyPart)
        {
            if (bodyPart == null)
            {
                return;
            }
            
            // Add body part stats incStat with the percent too incPercentStat
            // Accessed by bodyPart.stats.inc...
            Health += bodyPart.stats.incHealth + (Health * bodyPart.stats.incPercentHealth);
            Mana += bodyPart.stats.incMana + (Mana * bodyPart.stats.incPercentMana);
            DamageReduction += bodyPart.stats.incDamageReduction + (DamageReduction * bodyPart.stats.incPercentDamageReduction);
            Attack += bodyPart.stats.incAttack + (Attack * bodyPart.stats.incPercentAttack);
            Speed += bodyPart.stats.incSpeed + (Speed * bodyPart.stats.incPercentSpeed);
            AbilityPower += bodyPart.stats.incAbilityPower + (AbilityPower * bodyPart.stats.incPercentAbilityPower);
            Evasion += bodyPart.stats.incEvasion + (Evasion * bodyPart.stats.incPercentEvasion);
            CriticalChance += bodyPart.stats.incCriticalChance + (CriticalChance * bodyPart.stats.incPercentCriticalChance);
            CriticalDamage += bodyPart.stats.incCriticalDamage + (CriticalDamage * bodyPart.stats.incPercentCriticalDamage);
            Accuracy += bodyPart.stats.incAccuracy + (Accuracy * bodyPart.stats.incPercentAccuracy);
        }

        // function to clone the HomunStats
        public HomunStats Clone()
        {
            HomunStats clone = new HomunStats();
            clone.Health = Health;
            clone.Mana = Mana;
            clone.DamageReduction = DamageReduction;
            clone.Attack = Attack;
            clone.Speed = Speed;
            clone.AbilityPower = AbilityPower;
            clone.Evasion = Evasion;
            clone.CriticalChance = CriticalChance;
            clone.CriticalDamage = CriticalDamage;
            return clone;
        }
        
    }
}