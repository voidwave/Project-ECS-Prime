
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class MovementSystem : JobComponentSystem
{
    

    [Unity.Burst.BurstCompile]
    struct UnitMovementJob : IJobProcessComponentData<Position, Rotation, UnitStats, BotAI>
    {
        public float deltaTime;

        public void Execute(ref Position position, ref Rotation rotation, [ReadOnly] ref UnitStats unitStats, [ReadOnly] ref BotAI ai)
        {

            if (ai.MoveDir.x == 0 && ai.MoveDir.z == 0)
                return;

            float3 value = position.Value;
            float3 dir = new float3(ai.MoveDir.x, 0, ai.MoveDir.z);
            dir = math.normalize(dir);
            rotation.Value = Quaternion.LookRotation(dir);
            value += deltaTime * unitStats.MovementSpeed.Value * math.forward(rotation.Value);
            position.Value = value;

        }

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        

        UnitMovementJob moveJob = new UnitMovementJob
        {
            deltaTime = Time.deltaTime
        };
        JobHandle MoveHandle = moveJob.Schedule(this, inputDeps);

        return MoveHandle;
    }

}
