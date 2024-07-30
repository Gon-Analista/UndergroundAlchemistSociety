using System;
using UnityEngine;

namespace Script.BodyParts
{
    public class BodyPartDisplay : MonoBehaviour
    {
        public BodyPart bodyPart;
        public bool isFlipped;
        private SpriteRenderer _spriteRenderer;
   
        void Start()
        {
            // Initialize the SpriteRenderer component
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            // Set the sprite and color from the bodyPart
            if (bodyPart != null)
            {
                _spriteRenderer.sprite = bodyPart.sprite;
                _spriteRenderer.color = bodyPart.color;
                transform.localScale = new Vector3(transform.localScale.x * 4, transform.localScale.y * 4, transform.localScale.z);
            }

            // Handle flipping the sprite if isFlipped is true
            if (isFlipped)
            {
                _spriteRenderer.flipX = true;
            }
        }

        void Update()
        {
            // Update the sprite and color if they change at runtime
            if (bodyPart != null)
            {
                if (_spriteRenderer.sprite != bodyPart.sprite)
                {
                    _spriteRenderer.sprite = bodyPart.sprite;
                }

                if (_spriteRenderer.color != bodyPart.color)
                {
                    _spriteRenderer.color = bodyPart.color;
                }
            }

            // Handle flipping the sprite if isFlipped changes
            _spriteRenderer.flipX = isFlipped;
        }
        
        public void SetBodyPart(BodyPart newBodyPart, bool flip)
        {
            bodyPart = newBodyPart;
            isFlipped = flip;

            // Update the sprite and color immediately
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
                if (_spriteRenderer == null)
                {
                    _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                }
            }

            if (bodyPart != null)
            {
                _spriteRenderer.sprite = bodyPart.sprite;
                _spriteRenderer.color = bodyPart.color;
            }

            var sortingOrder = GetBodyTypeSortingOrder();
            _spriteRenderer.flipX = isFlipped;
            _spriteRenderer.sortingOrder = sortingOrder;
            //transform.localPosition = new Vector3(transform.localPosition.x, yPosition, transform.localPosition.z);
        }

        // Return the sortingOrder and the y position of the sprite
        private int GetBodyTypeSortingOrder()
        {
            switch (bodyPart.partType)
            {
                case BodyPartType.Core:
                    return 10;
                case BodyPartType.Legs:
                    return 11;
                case BodyPartType.Arms:
                    return 12;
                case BodyPartType.Accessory:
                default:
                    return 10;
            }
        }
    }
}