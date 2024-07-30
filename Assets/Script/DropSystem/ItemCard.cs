using System;
using Script.BodyParts;
using Script.Loaders;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;


namespace Script.DropSystem
{
    [Serializable]
    public class ItemCard : MonoBehaviour
    {
        public BodyPart bodyPart;

                 switch (bodyPart.partType)
                 {
                     // Change the positions of the image for each body part
                        case BodyPartType.Legs:
                            buttonImage.rectTransform.anchoredPosition = new Vector2(-35, 140);
                            break;
                        case BodyPartType.Accessory:
                            buttonImage.rectTransform.anchoredPosition = new Vector2(-149, 3.8f);
                            break;
                        case BodyPartType.Arms:
                        case BodyPartType.Core:
                        default:
                            break;
                 }

                 RectTransform rectTransform = itemCardButton.GetComponent<RectTransform>();
                 rectTransform.anchoredPosition += localPosition;
                 
                 // On Click event
                 Button buttonComponent = itemCardButton.GetComponent<Button>();
                 buttonComponent.onClick.AddListener(() =>
                 {
                     GameManager.Instance.SelectDropAndContinue(bodyPart.id);
                 });
             }
             return null;
         }   
    }
}
