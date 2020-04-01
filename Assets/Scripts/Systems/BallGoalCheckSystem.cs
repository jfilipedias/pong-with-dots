﻿using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class BallGoalCheckSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities
            .WithAll<BallTag>()
            .WithoutBurst()
            .ForEach((Entity entity, in Translation translation) =>
        {
            float3 position = translation.Value;
            float bound = GameManager.main.xBound;

            if (position.x >= bound)
            {
                GameManager.main.PlayerScored(0);
                ecb.DestroyEntity(entity);
            }
            else if (position.x <= -bound)
            {
                GameManager.main.PlayerScored(1);
                ecb.DestroyEntity(entity);
            }
        }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;  
    }
}
