using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class MovementSystem : JobComponentSystem
{
    

    [Unity.Burst.BurstCompile]
    struct UnitMovementJob : IJobProcessComponentData<Position, Rotation, MovementSpeed, Heading>
    {
        public float deltaTime;

        public void Execute(ref Position position, ref Rotation rotation, [ReadOnly] ref MovementSpeed speed, [ReadOnly] ref Heading heading)
        {

            if (heading.Value.x == 0 && heading.Value.z == 0)
                return;

            float3 value = position.Value;
            float3 dir = new float3(heading.Value.x, 0, heading.Value.z);
            dir = math.normalize(dir);
            rotation.Value = Quaternion.LookRotation(dir);
            value += deltaTime * speed.Value * math.forward(rotation.Value);
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
