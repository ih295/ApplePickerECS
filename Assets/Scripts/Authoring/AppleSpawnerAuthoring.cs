using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System;

public struct AppleSpawner : IComponentData
{
    public Entity Prefab;
    public Entity BadApplePrefab;
    public bool SpawnBadApplesRandomly;
    public float Interval;
    public uint Seed;
    public Unity.Mathematics.Random Rand;
}

[DisallowMultipleComponent]
public class AppleSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject badApplePrefab;
    [SerializeField] private bool spawnBadApplesRandomly = false;
    [SerializeField] private float appleSpawnInterval = 1f;
    [SerializeField] private uint seed = (uint) 2405429942;

    private class AppleSpawnerAuthoringBaker : Baker<AppleSpawnerAuthoring>
    {
        public override void Bake(AppleSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            authoring.seed += (uint) DateTimeOffset.Now.ToUnixTimeSeconds();
            var rand = new Unity.Mathematics.Random((uint) authoring.seed);
            //Create the spawner
            AddComponent(entity, new AppleSpawner
            {
                Prefab = GetEntity(authoring.applePrefab, TransformUsageFlags.Dynamic),
                BadApplePrefab = GetEntity(authoring.badApplePrefab, TransformUsageFlags.Dynamic),
                Interval = authoring.appleSpawnInterval,
                SpawnBadApplesRandomly = authoring.spawnBadApplesRandomly,
                Seed = authoring.seed,
                Rand = rand
            });
            AddComponent(entity, new Timer { Value = 2f });
        }
    }
}