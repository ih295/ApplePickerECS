using Unity.Entities;
using UnityEngine;

public struct BasketTag : IComponentData
{
}

public struct BasketIndex : IComponentData
{
    public int Value;
}

public struct DestroyBasketTag : IComponentData
{
}

public class BasketAuthoring : MonoBehaviour
{
    private class BasketAuthoringBaker : Baker<BasketAuthoring>
    {
        public override void Bake(BasketAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<BasketTag>(entity);
            AddComponent<MoveWithMouse>(entity);
        }
    }
}
