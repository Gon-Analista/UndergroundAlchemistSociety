using System.Collections.Generic;
using Script.Modifiers;
using TMPro;
using UnityEngine;
using Script.Colosseum;

namespace Script.UI
{
    public class FightUIManager : MonoBehaviour
    {
        // UI Stuff
        public TextMeshProUGUI playerChargeText;
        public TextMeshProUGUI enemyChargeText;
        
        public TextMeshProUGUI playerHealthText;
        public TextMeshProUGUI enemyHealthText;
        
        public TextMeshProUGUI playerModifiersText;
        public TextMeshProUGUI enemyModifiersText;

        public HomunFight currentFight;

        private void Update()
        {
            if (currentFight?.enemyFighter == null || currentFight?.playerFighter == null)
            {
                return;
            }
            playerChargeText.text = $"Player: {(currentFight.playerCharge * 100):0}%";
            enemyChargeText.text = $"Enemy: {(currentFight.enemyCharge * 100):0}%";
            
            playerHealthText.text = $"Player: {currentFight.GetPlayerHealth()}";
            enemyHealthText.text = $"Enemy: {currentFight.GetEnemyHealth()}";
            
            // Update player modifiers
            var playerModifiers = currentFight.GetPlayerModifiers();
            playerModifiersText.text = "Player: ";
            var playerModifierCounts = CountModifiers(playerModifiers);
            foreach (var kvp in playerModifierCounts)
            {
                playerModifiersText.text += $"{ConvertStatusModifierToString(kvp.Key)} ({kvp.Value})\n";
            }
            
            // Update enemy modifiers
            var enemyModifiers = currentFight.GetEnemyModifiers();
            enemyModifiersText.text = "Enemy: ";
            var enemyModifierCounts = CountModifiers(enemyModifiers);
            foreach (var kvp in enemyModifierCounts)
            {
                enemyModifiersText.text += $"{ConvertStatusModifierToString(kvp.Key)} ({kvp.Value})\n";
            }
        }

        private Dictionary<StatusModifier, int> CountModifiers(List<ModifierData> modifiers)
        {
            var modifierCounts = new Dictionary<StatusModifier, int>();
            foreach (var modifier in modifiers)
            {
                if (modifierCounts.ContainsKey(modifier.modifier))
                {
                    modifierCounts[modifier.modifier]++;
                }
                else
                {
                    modifierCounts[modifier.modifier] = 1;
                }
            }
            return modifierCounts;
        }

        private string ConvertStatusModifierToString(StatusModifier modifier)
        {
            // Suponiendo que StatusModifier es una enumeración, convertirla a string
            return modifier.ToString();
        }
    }
}
