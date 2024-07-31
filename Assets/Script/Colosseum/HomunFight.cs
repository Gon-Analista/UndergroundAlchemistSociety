using System.Collections.Generic;
using Script.Loaders;
using Script.Modifiers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Colosseum
{
    public enum FightResult
    {
        Won,
        Lost,
        OnGoing
    }
    
    public class HomunFight : MonoBehaviour
    {
        public Homun.Homun playerFighter;
        public Homun.Homun enemyFighter;
        public float playerCharge;
        public float enemyCharge;

        // Multiplier to slow down the charge rate
        public float chargeSpeedMultiplier = 0.5f; // Ajustar este valor según sea necesario
        public FightResult result = FightResult.OnGoing;
        
        public float timer;
        
        // Start is called before the first frame update
        private void Start()
        {
            // Take GameManager's homun as the player, and add it to the GameObject's children.
            var playerHomunPosMarker = GameObject.Find("PlayerHomun");
            playerFighter = playerHomunPosMarker.AddComponent<Homun.Homun>();
            playerFighter.Clone(GameManager.Instance.homun);
            playerFighter.isPlayer = true;
            
            // Set the first value of enemyFighters to EnemyHomun GameObject
            var enemyHomunPosMarker = GameObject.Find("EnemyHomun");
            enemyFighter = Homun.Homun.CreateRandomHomun(GameManager.Instance.CalculateDifficulty(), enemyHomunPosMarker.gameObject);
            result = FightResult.OnGoing;
            timer = 60;
        }

        public int GetPlayerHealth()
        {
            return (int)playerFighter.GetStats().Health;
        }
        
        public int GetEnemyHealth()
        {
            return (int)enemyFighter.GetStats().Health;
        }
        
        public List<ModifierData> GetPlayerModifiers()
        {
            return playerFighter.GetActiveStatusModifiers();
        }
        
        public List<ModifierData> GetEnemyModifiers()
        {
            return enemyFighter.GetActiveStatusModifiers();
        }
        
        // Update is called once per frame
        void Update()
        {
            // Just a random check to avoid bugs
            if (playerFighter == null || enemyFighter == null)
            {
                return;
            }
            
            // Check if the player has enough health to continue
            if (playerFighter.Stats.Health <= 0)
            {
                // Player lost
                result = FightResult.Lost;
                return;
            }
            
            // Check if there's any enemy alive
            if (enemyFighter.Stats.Health <= 0)
            {
                // Player won
                result = FightResult.Won;
                return;
            }
            
            // Update timer
            timer -= Time.deltaTime;
            // If timer reaches <= 0, the one with most hp wins
            if (timer <= 0)
            {
                result = playerFighter.Stats.Health > enemyFighter.Stats.Health ? FightResult.Won : FightResult.Lost;
                return;
            }
            
            // Update charge using the Homun's Speed stat and the chargeSpeedMultiplier
            playerCharge += Time.deltaTime * playerFighter.GetStats().Speed * chargeSpeedMultiplier;
            enemyCharge += Time.deltaTime * enemyFighter.GetStats().Speed * chargeSpeedMultiplier;
            
            // Check if the player and enemy are able to attack. If they attack at the same time, 50% chance of the player attacking first.
            var isEnemyAttacking = enemyCharge >= 1;
            var isPlayerAttacking = playerCharge >= 1;

            if (isEnemyAttacking && isPlayerAttacking)
            {
                if (Random.value < 0.5f)
                {
                    playerFighter.Attack(enemyFighter);
                    playerCharge = 0;
                }
                else
                {
                    enemyFighter.Attack(playerFighter);
                    enemyCharge = 0;
                }
            }
            else if (isEnemyAttacking)
            {
                enemyFighter.Attack(playerFighter);
                enemyCharge = 0;
            }
            else if (isPlayerAttacking)
            {
                playerFighter.Attack(enemyFighter);
                playerCharge = 0;
            }
        }
    }
}
