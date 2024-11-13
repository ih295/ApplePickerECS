using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct TreeMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // foreach (var (transform, speed, bounds) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleTreeSpeed>, RefRO<AppleTreeBounds>>())
        // {
        //     var position = transform.ValueRO.Position;
        //     position.x += speed.ValueRO.Value * deltaTime;
        //     transform.ValueRW.Position = position;
        //
        //     if (position.x > bounds.ValueRO.Right)
        //     {
        //         speed.ValueRW.Value = -math.abs(speed.ValueRO.Value);
        //     }
        //     else if (position.x < bounds.ValueRO.Left)
        //     {
        //         speed.ValueRW.Value = math.abs(speed.ValueRO.Value);
        //     }
        // }

        new MoveTreeJob { DeltaTime = deltaTime}.Schedule();
    }

    [BurstCompile]
    private partial struct MoveTreeJob : IJobEntity
    {
        public float DeltaTime;

        [BurstCompile]
        private void Execute(ref LocalTransform transform, ref AppleTreeSpeed speed, in AppleTreeBounds bounds)
        {
            transform.Position.x += speed.Value * DeltaTime;
            if(bounds.Tag.MoveOnYAxis)
            {
                //Move tree on y-axis
                transform.Position.y += (int) speed.Value * DeltaTime;
            }

            if (transform.Position.x > bounds.Right)
            {
                speed.Value = -math.abs(speed.Value);
            }
            else if (transform.Position.x < bounds.Left)
            {
                speed.Value = math.abs(speed.Value);
            }
        }
    }
}
