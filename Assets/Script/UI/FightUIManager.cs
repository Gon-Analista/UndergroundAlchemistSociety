using System;
using System.Collections.Generic;
using Script.Modifiers;
using TMPro;
using UnityEngine;
using Script.Colosseum;
using Script.Loaders;
using UnityEngine.UI;

namespace Script.UI
{
    public class FightUIManager : MonoBehaviour
    {
        // UI Stuff
        public HomunFight currentFight;

        public GameObject panelPlayer;
        public GameObject panelEnemy;

        public TextMeshProUGUI playerAttack;
        public TextMeshProUGUI enemyAttack;
        public TextMeshProUGUI playerDefense;
        public TextMeshProUGUI enemyDefense;
        public TextMeshProUGUI playerHpValue;
        public TextMeshProUGUI enemyHpValue;
        public TextMeshProUGUI playerChargeValue;
        public TextMeshProUGUI enemyChargeValue;
        public TextMeshProUGUI timer;

        private Dictionary<StatusModifier, GameObject> _enemyModifiers;
        private Dictionary<StatusModifier, GameObject> _playerModifiers;
        
        // Markers
        public Image fight01;
        public Image fight02;
        public Image fight03;
        public Image fight04;
        public Image bossFight;

        private void Update()
        {
            if (currentFight?.enemyFighter == null || currentFight?.playerFighter == null)
            {
                return;
            }
            
            timer.text = $"{Mathf.Max(currentFight.timer,0):0}";
            
            // Update player and enemy health, in percent
            playerHpValue.text = $"{currentFight.GetPlayerHealth()}";
            enemyHpValue.text = $"{currentFight.GetEnemyHealth()}";
            playerChargeValue.text = $"{(currentFight.playerCharge * 100):0}%";
            enemyChargeValue.text = $"{(currentFight.enemyCharge* 100):0}%";

            // Update player and enemy attack and defense
            playerAttack.text = $"{currentFight.playerFighter.GetStats().Attack}";
            enemyAttack.text = $"{currentFight.enemyFighter.GetStats().Attack}";
            playerDefense.text = $"{currentFight.playerFighter.GetStats().DamageReduction}";
            enemyDefense.text = $"{currentFight.playerFighter.GetStats().DamageReduction}";

            // Update player modifiers
            var playerModifiers = currentFight.GetPlayerModifiers();
            var playerModifierCounts = CountModifiers(playerModifiers);
            UpdateModifiers(panelPlayer, playerModifierCounts);
            
            var enemyModifiers = currentFight.GetEnemyModifiers();
            var enemyModifierCounts = CountModifiers(enemyModifiers);
            UpdateModifiers(panelEnemy, enemyModifierCounts);
        }

        private void UpdateModifiers(GameObject modifierPanel, Dictionary<StatusModifier, int> modifierCounts)
        {
            // DEstroy all of the children of modifierPanel
            foreach (Transform child in modifierPanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            int iconPositionX = 0;
            int iconPositionY = 0;
            int iconsPerRow = Mathf.FloorToInt(modifierPanel.GetComponent<RectTransform>().rect.width / 32); // Adjust icons per row based on panel width
            
            foreach (var kvp in modifierCounts)
            {
                // Instantiate the prefab "ModifierIcon"
                var modifierIcon = Instantiate(Resources.Load<GameObject>("ModifierIcon"), modifierPanel.transform);
                var modifierMultiplier = modifierIcon.GetComponentInChildren<TextMeshProUGUI>();
                modifierMultiplier.text = $"x{kvp.Value}";
                // Set the icon sprite
                var iconName = kvp.Key switch
                {
                    StatusModifier.Bleed => "icon_bleed",
                    StatusModifier.Slow => "icon_slow",
                    StatusModifier.Stun => "icon_stun",
                    StatusModifier.Paralyze => "icon_paralyze",
                    StatusModifier.Poison => "icon_poison",
                    StatusModifier.Weakness => "icon_weakness",
                    StatusModifier.Vulnerability => "icon_vulnerability",
                    StatusModifier.Burn => "icon_burn",
                    StatusModifier.Blind => "icon_blind",
                    _ => "icon_attack",
                };

                // Get the Image component from the prefab
                var iconImage = modifierIcon.GetComponentInChildren<Image>();
                // set the image to the iconName
                iconImage.sprite = Resources.Load<Sprite>($"Icons/{iconName}");
                // Set the position of the modifierIcon inside the panel, beginning in the top left
                var rectTransform = modifierIcon.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(iconPositionX * 32, -iconPositionY * 32);

                iconPositionX++;
                if (iconPositionX >= iconsPerRow)
                {
                    iconPositionX = 0;
                    iconPositionY++;
                }
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

        private void Start()
        {
            var currentRound = GameManager.Instance.round + 1;
            if (currentRound >= 5)
                bossFight.sprite = Resources.Load<Sprite>("Icons/icon_boss_done");
            if (currentRound >= 4)
                fight04.sprite = Resources.Load<Sprite>("Icons/icon_fight_done");
            if (currentRound >= 3)
                fight03.sprite = Resources.Load<Sprite>("Icons/icon_fight_done");
            if (currentRound >= 2)
                fight02.sprite = Resources.Load<Sprite>("Icons/icon_fight_done");
            if (currentRound >= 1)
                fight01.sprite = Resources.Load<Sprite>("Icons/icon_fight_done");
        }
    }
}
