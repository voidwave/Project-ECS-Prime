
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
        public ComponentDataArray<Heading> headings;
        [ReadOnly] public ComponentDataArray<Position> positions;
        //[ReadOnly] public ComponentDataArray<Team> teams;
        //public float dt;

        public void Execute(int i)
        {
            //i=0 is player
            if (i == 0)
                return;

            float3 dir = new float3(0, 0, 0);
            float detectionRange = 10;
            //float closestEnemy = 100;
            //int targetID = -1;

            // for (int j = 0; j < teams.Length; j++)
            // {
            //     if (teams[j].Value == teams[i].Value)
            //         continue;

            float distance = math.distance(positions[0].Value, positions[i].Value);

            if (distance < detectionRange)
                dir = positions[0].Value - positions[i].Value;

            //if (distance > closestEnemy)
            //    continue;

            //closestEnemy = distance;

            //targetID = 0;
            // }

            headings[i] = new Heading
            {
                Value = dir
            };


        }
    }

    ComponentGroup m_EnemyAIGroup;

    protected override void OnCreateManager()
    {
        m_EnemyAIGroup = GetComponentGroup(
            ComponentType.ReadOnly(typeof(Position)),
            //ComponentType.ReadOnly(typeof(Team)),
            typeof(Heading));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var aiJob = new EnemyAIJob
        {
            positions = m_EnemyAIGroup.GetComponentDataArray<Position>(),
            //teams = m_EnemyAIGroup.GetComponentDataArray<Team>(),
            headings = m_EnemyAIGroup.GetComponentDataArray<Heading>()
        };
        var aiJobHandle = aiJob.Schedule(m_EnemyAIGroup.CalculateLength(), 64, inputDeps);
        return aiJobHandle;
    }
}


