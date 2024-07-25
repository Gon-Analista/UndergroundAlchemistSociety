using System;
using Script.Colosseum;
using TMPro;
using UnityEngine;

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
            playerChargeText.text = $"Player: {(currentFight.playerCharge * 100):0}%";
            enemyChargeText.text = $"Enemy: {(currentFight.enemyCharge * 100):0}%";
            
            playerHealthText.text = $"Player: {currentFight.GetPlayerHealth()}";
            enemyHealthText.text = $"Enemy: {currentFight.GetEnemyHealth()}";
            
            // Get the player's ModifierData, get the name and display them:
            var playerModifiers = currentFight.GetPlayerModifiers();
            playerModifiersText.text = "Player: ";
            foreach (var modifier in playerModifiers)
            {
                playerModifiersText.text += modifier.modifier + "\n";
            }
            
            // Now the same but for the enemy
            var enemyModifiers = currentFight.GetEnemyModifiers();
            enemyModifiersText.text = "Enemy: ";
            foreach (var modifier in enemyModifiers)
            {
                enemyModifiersText.text += modifier.modifier + "\n";
            }
        }
    }
}