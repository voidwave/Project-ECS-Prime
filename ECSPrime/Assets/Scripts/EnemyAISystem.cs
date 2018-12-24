
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class EnemyAISystem : JobComponentSystem
{
    [Unity.Burst.BurstCompile]
    struct EnemyAIJob : IJobParallelFor
    {
        public ComponentDataArray<BotAI> botAIs;
        [ReadOnly] public ComponentDataArray<Position> positions;
        [ReadOnly] public ComponentDataArray<Team> teams;
        //public float dt;

        public void Execute(int i)
        {
            if (teams[i].Value == 0)
                return;

            float3 dir = new float3(0, 0, 0);
            float detectionRange = 10;
            float closestEnemy = 100;
            int targetID = -1;
            for (int j = 0; j < teams.Length; j++)
            {
                if (teams[j].Value == teams[i].Value)
                    continue;

                float distance = math.distance(positions[j].Value, positions[i].Value);

                if (distance > detectionRange)
                    continue;

                if (distance > closestEnemy)
                    continue;

                closestEnemy = distance;
                dir = positions[j].Value - positions[i].Value;
                targetID = j;
            }

            botAIs[i] = new BotAI
            {
                MoveDir = dir,
                MousePosition = new float3(0, 0, 0),
                targetIndex = targetID
            };


        }
    }

    ComponentGroup m_EnemyAIGroup;

    protected override void OnCreateManager()
    {
        m_EnemyAIGroup = GetComponentGroup(
            ComponentType.ReadOnly(typeof(Position)),
            ComponentType.ReadOnly(typeof(Team)),
            typeof(BotAI));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var aiJob = new EnemyAIJob
        {
            positions = m_EnemyAIGroup.GetComponentDataArray<Position>(),
            teams = m_EnemyAIGroup.GetComponentDataArray<Team>(),
            botAIs = m_EnemyAIGroup.GetComponentDataArray<BotAI>()
        };
        var aiJobHandle = aiJob.Schedule(m_EnemyAIGroup.CalculateLength(), 64, inputDeps);
        return aiJobHandle;
    }
}


