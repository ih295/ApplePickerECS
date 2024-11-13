using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct AppleTreeTag : IComponentData
{
    public bool MoveOnYAxis;
}

public struct AppleTreeSpeed : IComponentData
{
    public float Value;
}

public struct AppleTreeBounds : IComponentData
{
    public float Left;
    public float Right;
    public AppleTreeTag Tag;
}

public struct AppleTreeDirectionChangeChance : IComponentData
{
    public float Value;
}

public struct AppleTreeRandom : IComponentData
{
    public Random Value;
}

public class AppleTreeAuthoring : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float leftAndRightEdge = 24f;
    [SerializeField] private float directionChangeChance = 0.1f;
    [SerializeField] private bool moveOnYAxis = false;

    private class AppleTreeAuthoringBaker : Baker<AppleTreeAuthoring>
    {
        public override void Bake(AppleTreeAuthoring authoring)
        {
            // TransformUsageFlags specifies how the entity's transform will be used.
            // - None: The entity will not have a transform component.
            // - Renderable: The entity will have a transform component but will not be updated during runtime.
            // - Dynamic: The entity will have a transform component and will be updated during runtime.
            // - WorldSpace: Indicates that the entity's transform is in world space.
            // - NonUniformScale: Indicates that the entity's transform has non-uniform scale.
            // - ManualOverride: Use it only if you want to take control of the entity's transform.

            // The apple tree entity will have a transform component and will be updated during runtime.
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var tag = new AppleTreeTag {MoveOnYAxis = authoring.moveOnYAxis};
            AddComponent(entity, tag);

            AddComponent(entity, new AppleTreeSpeed { Value = authoring.speed });
            AddComponent(entity, new AppleTreeBounds
            {
                Left = -authoring.leftAndRightEdge,
                Right = authoring.leftAndRightEdge,
                Tag = tag
            });

            AddComponent(entity, new AppleTreeDirectionChangeChance
            {
                Value = authoring.directionChangeChance
            });
            AddComponent(entity, new AppleTreeRandom
            {
                Value = Random.CreateFromIndex((uint)entity.Index)
            });
        }
    }
}
