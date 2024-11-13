using Unity.Entities;
using UnityEngine;

public struct BasketSpawner : IComponentData
{
    public Entity BasketPrefab;
    public int Count;
    public float BottomY;
    public float Spacing;
}

[DisallowMultipleComponent]
public class BasketSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject basketPrefab;
    [SerializeField] private int count = 3;
    [SerializeField] private float bottomY = -14f;
    [SerializeField] private float spacing = 2f;

    private class BasketSpawnerAuthoringBaker : Baker<BasketSpawnerAuthoring>
    {
        public override void Bake(BasketSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BasketSpawner
            {
                BasketPrefab = GetEntity(authoring.basketPrefab, TransformUsageFlags.Dynamic),
                Count = authoring.count,
                BottomY = authoring.bottomY,
                Spacing = authoring.spacing
            });
        }
    }
}
