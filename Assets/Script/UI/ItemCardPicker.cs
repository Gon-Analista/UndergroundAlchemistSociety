using System;
using System.Collections.Generic;
using Script.BodyParts;
using Script.DropSystem;
using Script.Loaders;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.UI
{
    public class ItemCardPicker : MonoBehaviour
    {
        private List<ItemCard> Cards = new List<ItemCard>();
        public List<BodyPart> bodyParts;
        private static readonly List<Vector2> CardPositions = new List<Vector2>
        {
            new Vector2(-390, 0),
            new Vector2(20, 0),
            new Vector2(430, 0)
        };
        
        private void Start()
        {
            bodyParts = GameManager.Instance.currentPrizes;
            
            foreach (var part in bodyParts)
            {
                ItemCard card = ItemCard.CreateItemCardPicker(part, transform);
                Cards.Add(card);
                card.InitializeCardButton(CardPositions[bodyParts.IndexOf(part)]);
            }
        }
    }
}