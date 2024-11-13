using Unity.Entities;

public struct PlayerScore : IComponentData
{
    public int Value;
}

public partial class UpdateScoreSystem : SystemBase
{
    private EntityQuery m_PlayerScoreQuery;

    protected override void OnCreate()
    {
        m_PlayerScoreQuery = GetEntityQuery(typeof(PlayerScore));
    }

    protected override void OnUpdate()
    {
        if (ScoreCounter.Instance == null)
            return;

        if (m_PlayerScoreQuery.CalculateEntityCount() == 0)
            EntityManager.CreateEntity(typeof(PlayerScore));

        var score = EntityManager.GetComponentData<PlayerScore>(m_PlayerScoreQuery.GetSingletonEntity());
        ScoreCounter.Instance.UpdateScore(score.Value);
    }
}
