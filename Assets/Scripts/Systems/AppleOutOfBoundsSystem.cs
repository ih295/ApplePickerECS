using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class AppleOutOfBoundsSystem : SystemBase
{
    private EntityQuery m_BasketQuery;
    private bool basketsInitialized = false;

    protected override void OnCreate()
    {
        m_BasketQuery = GetEntityQuery(typeof(BasketIndex));
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var basketCount = m_BasketQuery.CalculateEntityCount();
        var didMiss = false;

        foreach (var (transform, bottomY, apple) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<AppleBottomY>>().WithEntityAccess())
        {
            if (transform.ValueRO.Position.y < bottomY.ValueRO.Value)
            {
                ecb.DestroyEntity(apple);
                //If this is a good apple, count the miss
                if(bottomY.ValueRO.Tag.ScoreIncrement >= 0)
                {
                    didMiss = true;
                }
            }
        }

        //Return to main menu if no baskets are left
        if(basketCount > 0)
        {
            //Baskets have been initialized
            basketsInitialized = true;
        }
        if((basketCount < 1) && basketsInitialized)
        {
            //Return to main menu
            SceneManager.LoadScene(0);
        }
        if (didMiss)
        {

            foreach (var (index, basket) in SystemAPI.Query<RefRO<BasketIndex>>().WithEntityAccess())
            {
                if (index.ValueRO.Value == basketCount - 1)
                {
                    ecb.DestroyEntity(basket);
                }
            }

            // destroy all apples
            foreach (var (_, apple) in SystemAPI.Query<RefRO<AppleTag>>().WithEntityAccess())
            {
                ecb.DestroyEntity(apple);
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
