using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// Systems are classified into groups.
// - InitializationSystemGroup: Systems run once at the start of the game.          -> for Start
// - SimulationSystemGroup: Systems run every frame. (default)                      -> for Update
// - PresentationSystemGroup: Systems run every frame after SimulationSystemGroup.  -> for LateUpdate
// - FixedStepSimulationGroup: Systems run every fixed time step.                   -> for FixedUpdate

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct BasketSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<BasketSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Use the Begin Initialization Entity Command Buffer System to create entities during the start of the
        // initialization phase

        // Begin -> Start of the frame (best for instantiating entities)
        // End -> End of the frame     (best for destroying entities)

        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var spawner = SystemAPI.GetSingleton<BasketSpawner>();

        for (var i = 0; i < spawner.Count; i++)
        {
            var basket = ecb.Instantiate(spawner.BasketPrefab);
            var position = new float3 { y = spawner.BottomY + spawner.Spacing * i };
            ecb.SetComponent(basket, LocalTransform.FromPosition(position));
            ecb.AddComponent(basket, new BasketIndex { Value = i });
        }

        // Destroy the spawner entity after spawning the baskets
        ecb.DestroyEntity(SystemAPI.GetSingletonEntity<BasketSpawner>());
    }
}