using Unity.Entities;
using UnityEngine;

// Empty components can be used to tag entities
public struct AppleTag : IComponentData
{
    public int ScoreIncrement;
}

public struct AppleBottomY : IComponentData
{
    // If you have only one field in a component, name it "Value"

    public float Value;
    public AppleTag Tag;
}

[DisallowMultipleComponent]
public class AppleAuthoring : MonoBehaviour
{
    [SerializeField] private float bottomY = -14f;
    [SerializeField] private int scoreIncrement = 100;

    private class AppleAuthoringBaker : Baker<AppleAuthoring>
    {
        public override void Bake(AppleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var appleTag = new AppleTag{ScoreIncrement = authoring.scoreIncrement};
            AddComponent(entity, new AppleTag {ScoreIncrement = authoring.scoreIncrement});
            AddComponent(entity, new AppleBottomY { Value = authoring.bottomY, Tag =  appleTag});
        }
    }
}
