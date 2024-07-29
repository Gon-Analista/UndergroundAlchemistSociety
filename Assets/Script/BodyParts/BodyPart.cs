using Script.Modifiers;
using UnityEngine;

namespace Script.BodyParts
{
    [CreateAssetMenu(fileName = "New Body Part", menuName = "Items/BodyPart")]
    public class BodyPart : ScriptableObject
    {
        public int id;
        public new string name;

        [SerializeField] public BodyPartType partType;
        [SerializeField] public Sprite sprite;
        [SerializeField] public Color color = Color.white;
        [SerializeField] public Texture2D maskTexture;
        
        [SerializeField] public BodyPartStats stats;

        public BodyPart(BodyPartType type)
        {
            partType = type;
            stats = new BodyPartStats();
        }
    }
}