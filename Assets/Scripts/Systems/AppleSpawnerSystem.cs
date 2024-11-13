using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;

[UpdateAfter(typeof(TimerSystem))]
public partial struct AppleSpawnerSystem : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        new SpawnJob { ECB = ecb}.Schedule();
    }

    [BurstCompile]
    private partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public Unity.Mathematics.Random Rand;

        private void Execute(in LocalTransform transform, ref AppleSpawner spawner, ref Timer timer)
        {
            Entity appleEntity;
            //Generate a random number to spawn bad apples randomly
            var spawnBadApple = spawner.Rand.NextInt(0, 100);
            spawner.Seed += 100;
            spawner.Rand = new Unity.Mathematics.Random(spawner.Seed);
            if (timer.Value > 0)
                return;

            timer.Value = spawner.Interval;

            //Spawn a bad apple randomly if requested by editor and spawnBadApple is even
            if(spawner.SpawnBadApplesRandomly && ((spawnBadApple % 2) == 0))
            {
                appleEntity = ECB.Instantiate(spawner.BadApplePrefab);
            }
            else
            {
                appleEntity = ECB.Instantiate(spawner.Prefab);
            }
            ECB.SetComponent(appleEntity, LocalTransform.FromPosition(transform.Position));
        }

    }
}