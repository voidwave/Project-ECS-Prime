
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class PlayerMovementSystem : JobComponentSystem
{
    [Unity.Burst.BurstCompile]
    struct PlayerMovementJob : IJobProcessComponentData<Position, Rotation, UnitStats, PlayerInput>
    {
        public float deltaTime;

        public void Execute(ref Position position, ref Rotation rotation, [ReadOnly] ref UnitStats unitStats, [ReadOnly] ref PlayerInput pi)
        {

            if (pi.MoveDir.x == 0 && pi.MoveDir.z == 0)
                return;

            float3 value = position.Value;
            float3 dir = new float3(pi.MoveDir.x, 0, pi.MoveDir.z);
            dir = math.normalize(dir);
            rotation.Value = Quaternion.LookRotation(dir);
            value += deltaTime * unitStats.MovementSpeed.Value * math.forward(rotation.Value);
            position.Value = value;

        }

    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        PlayerMovementJob playerMoveJob = new PlayerMovementJob
        {
            deltaTime = Time.deltaTime
        };
        JobHandle PlayerMoveHandle = playerMoveJob.Schedule(this, inputDeps);



        return PlayerMoveHandle;
    }

}
