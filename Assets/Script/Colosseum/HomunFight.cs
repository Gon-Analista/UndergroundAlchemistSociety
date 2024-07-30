using System.Collections.Generic;
using Script.Modifiers;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Colosseum
{
    public class HomunFight : MonoBehaviour
    {
        public Homun.Homun playerFighter;
        public List<Homun.Homun> enemyFighters;
        public float playerCharge;
        public float enemyCharge;

        // Multiplier to slow down the charge rate
        public float chargeSpeedMultiplier = 0.1f; // Ajustar este valor según sea necesario
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public int GetPlayerHealth()
        {
            return (int)playerFighter.GetStats().Health;
        }
        
        public int GetEnemyHealth()
        {
            return (int)enemyFighters[0].GetStats().Health;
        }
        
        public List<ModifierData> GetPlayerModifiers()
        {
            return playerFighter.GetActiveStatusModifiers();
        }
        
        public List<ModifierData> GetEnemyModifiers()
        {
            return enemyFighters[0].GetActiveStatusModifiers();
        }
        
        // Function to see if theres any enemy alive, and get its index
        private int GetAliveEnemy()
        {
            for (int i = 0; i < enemyFighters.Count; i++)
            {
                if (enemyFighters[i].Stats.Health > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        // Update is called once per frame
        void Update()
        {
            // Check if the player has enough health to continue
            if (playerFighter.Stats.Health <= 0)
            {
                // Player lost
                return;
            }
            
            // Check if there's any enemy alive
            var enemyIndex = GetAliveEnemy();
            if (enemyIndex == -1)
            {
                // Player won
                return;
            }
            
            // Update charge using the Homun's Speed stat and the chargeSpeedMultiplier
            playerCharge += Time.deltaTime * playerFighter.GetStats().Speed * chargeSpeedMultiplier;
            enemyCharge += Time.deltaTime * enemyFighters[0].GetStats().Speed * chargeSpeedMultiplier;
            
            // Check if the player and enemy are able to attack. If they attack at the same time, 50% chance of the player attacking first.
            var isEnemyAttacking = enemyCharge >= 1;
            var isPlayerAttacking = playerCharge >= 1;

            if (isEnemyAttacking && isPlayerAttacking)
            {
                if (Random.value < 0.5f)
                {
                    playerFighter.Attack(enemyFighters[enemyIndex]);
                    playerCharge = 0;
                }
                else
                {
                    enemyFighters[enemyIndex].Attack(playerFighter);
                    enemyCharge = 0;
                }
            }
            else if (isEnemyAttacking)
            {
                enemyFighters[enemyIndex].Attack(playerFighter);
                enemyCharge = 0;
            }
            else if (isPlayerAttacking)
            {
                playerFighter.Attack(enemyFighters[enemyIndex]);
                playerCharge = 0;
            }
        }
    }
}
