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
        [SerializeField] public BodyPartStats stats;

        public BodyPart(BodyPartType type)
        {
            partType = type;
            stats = new BodyPartStats();
        }
    }
}