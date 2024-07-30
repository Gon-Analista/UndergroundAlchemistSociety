using System;
using Script.BodyParts;
using UnityEngine;

namespace Script.UI
{
    public class ItemCardPicker : MonoBehaviour
    {
        public BodyPart bodyPart;

        private GameObject InitializeBodyPart(Vector2 localPosition)
        {
            if (bodyPart != null)
            {
                GameObject partObject = new GameObject(bodyPart.name);
                partObject.transform.parent = transform; // Set as child of Homun
                partObject.transform.localPosition = localPosition;

                SpriteRenderer spriteRenderer = partObject.AddComponent<SpriteRenderer>();
                BodyPartDisplay bodyPartDisplay = partObject.AddComponent<BodyPartDisplay>();
                bodyPartDisplay.SetBodyPart(bodyPart, false);

                return partObject;
            }
            return null;
        }
        
        public void Start()
        {
            
        }
    }
}