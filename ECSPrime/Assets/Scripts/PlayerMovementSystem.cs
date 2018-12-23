
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class PlayerMovementSystem : JobComponentSystem
{
    [ComputeJobOptimization]
    struct PlayerMovementJob : IJobProcessComponentData<Position, Rotation, MoveSpeed>
    {
        public float Horizontal;
        public float Vertical;
        public float deltaTime;

        public void Execute(ref Position position, ref Rotation rotation, [ReadOnly] ref MoveSpeed speed)
        {

            if (Horizontal == 0 && Vertical == 0)
                return;

            float3 value = position.Value;
            float3 dir = new float3(Horizontal, 0, Vertical);
            dir = math.normalize(dir);   
            rotation.Value = Quaternion.LookRotation(dir);
            value += deltaTime * speed.Value * math.forward(rotation.Value);
            position.Value = value;

        }

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        PlayerMovementJob moveJob = new PlayerMovementJob
        {
            Horizontal = Input.GetAxis("Horizontal"),
            Vertical = Input.GetAxis("Vertical"),
            deltaTime = Time.deltaTime
        };


        JobHandle moveHandle = moveJob.Schedule(this, inputDeps);

        return moveHandle;
    }

}
