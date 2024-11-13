using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial struct CollectAppleSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerScore>();
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var appleCount = new NativeArray<byte>(1, Allocator.TempJob);
        var appleScore = new NativeArray<int>(1, Allocator.TempJob);
        state.Dependency = new CollisionJob
        {
            AppleLookup = SystemAPI.GetComponentLookup<AppleTag>(true),
            BasketLookup = SystemAPI.GetComponentLookup<BasketTag>(true),
            ECB = ecb,
            AppleCount = appleCount,
            AppleScore = appleScore
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        if (appleCount[0] == 1)
        {
            var playerScore = SystemAPI.GetSingleton<PlayerScore>();
            playerScore.Value += appleScore[0];
            SystemAPI.SetSingleton(playerScore);
        }

        appleCount.Dispose();
    }

    [BurstCompile]
    private struct CollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<AppleTag> AppleLookup;
        [ReadOnly] public ComponentLookup<BasketTag> BasketLookup;

        public EntityCommandBuffer ECB;
        public NativeArray<byte> AppleCount;
        public NativeArray<int> AppleScore;

        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA; // basket
            var entityB = collisionEvent.EntityB; // apple
            if (AppleLookup.HasComponent(entityA) && BasketLookup.HasComponent(entityB))
            {
                //Is this a bad apple?
                if(AppleLookup[entityA].ScoreIncrement < 0)
                {
                    //Bad apple, drop one basket
                    ECB.DestroyEntity(entityB);
                }
                ECB.DestroyEntity(entityA);
                AppleCount[0] = 1;
                AppleScore[0] += AppleLookup[entityA].ScoreIncrement;
            }
            else if (AppleLookup.HasComponent(entityB) && BasketLookup.HasComponent(entityA))
            {
                ECB.DestroyEntity(entityB);
                AppleCount[0] = 1;
                AppleScore[0] += AppleLookup[entityB].ScoreIncrement;
            }
        }
    }
}