using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Physics;
public class MovementSystem : JobComponentSystem
{


    [BurstCompile]
    struct UnitMovementJob : IJobForEach<PhysicsVelocity, Rotation, MovementSpeed, Heading>
    {
        public float deltaTime;

        public void Execute(ref PhysicsVelocity velocity, ref Rotation rotation, [ReadOnly] ref MovementSpeed speed, [ReadOnly] ref Heading heading)
        {

            if (heading.Value.x == 0 && heading.Value.z == 0)
                return;


            float3 dir = new float3(heading.Value.x, 0, heading.Value.z);
            dir = math.normalize(dir);
            rotation.Value = Quaternion.LookRotation(dir);
            velocity.Linear = speed.Value * math.forward(rotation.Value);


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
