using System;
using Script.BodyParts;
using Script.Loaders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Script.DropSystem
{
    [Serializable]
    public class ItemCard : MonoBehaviour
    {
         public BodyPart bodyPart;
     
         public static ItemCard CreateItemCardPicker(BodyPart bodyPart, Transform parent)
         {
             GameObject itemCardPickerGameObject = new GameObject("ItemCardPicker_"+bodyPart.id);
             itemCardPickerGameObject.transform.parent = parent;
             itemCardPickerGameObject.transform.localPosition = Vector3.zero;
             itemCardPickerGameObject.transform.localScale = Vector3.one;
 
             ItemCard itemCardPicker = itemCardPickerGameObject.AddComponent<ItemCard>();
             itemCardPicker.bodyPart = bodyPart;
 
             return itemCardPicker;
         }
         
         public BodyPart ChooseBodyPart()
         {
             return bodyPart != null ? bodyPart : null;
         }
         
         public GameObject InitializeCardButton(Vector2 localPosition)
         {
             if (bodyPart != null)
             {
                 Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();   
                 GameObject itemCardButtonPrefab = Resources.Load<GameObject>("ItemCardButton");
                 GameObject itemCardButton = Instantiate(itemCardButtonPrefab, canvas.transform);
                 itemCardButton.name = "ItemCardButton_" + bodyPart.id;
                 
                 TextMeshProUGUI titleText = itemCardButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                 titleText.text = bodyPart.name;
 
                 TextMeshProUGUI descText = itemCardButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                 descText.text = bodyPart.description;
                 TextMeshProUGUI partTypeText = itemCardButton.transform.Find("PartType").GetComponent<TextMeshProUGUI>();
                 partTypeText.text = bodyPart.partType.ToString();
                 
                 Image buttonImage = itemCardButton.transform.Find("Image").GetComponent<Image>();
                 buttonImage.sprite = bodyPart.sprite;
                 buttonImage.color = bodyPart.color;

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
                 buttonComponent.onClick.AddListener(async () =>
                 {
                    AudioClip selectSound = Resources.Load<AudioClip>("Sounds/button");
                    AudioSource.PlayClipAtPoint(selectSound, Camera.main.transform.position, 0.17f);
                    await Task.Delay(1000);
                    GameManager.Instance.SelectDropAndContinue(bodyPart.id);
                 });
             }
             return null;
         }   
    }
}