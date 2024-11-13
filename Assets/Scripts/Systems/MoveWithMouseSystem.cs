using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public struct MoveWithMouse : IComponentData
{
}

public partial struct MoveWithMouseSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var mousePosition2D = Input.mousePosition;
        mousePosition2D.z = -Camera.main.transform.position.z;

        var mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);

        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<MoveWithMouse>())
        {
            var position = transform.ValueRO.Position;
            position.x = mousePosition3D.x;
            transform.ValueRW.Position = position;
        }
    }
}
