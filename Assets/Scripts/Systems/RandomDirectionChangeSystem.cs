using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct RandomDirectionChangeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new ChangeDirectionJob().Schedule();
    }

    [BurstCompile]
    private partial struct ChangeDirectionJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref AppleTreeRandom random, in AppleTreeDirectionChangeChance chance,
            ref AppleTreeSpeed speed)
        {
            if (random.Value.NextFloat() < chance.Value)
            {
                speed.Value *= -1;
            }
        }
    }
}
