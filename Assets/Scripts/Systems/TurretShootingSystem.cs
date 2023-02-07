using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

[BurstCompile]
partial struct TurretShootingSystem : ISystem
{
    ComponentLookup<WorldTransform> worldTransformLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        worldTransformLookup = state.GetComponentLookup<WorldTransform>(true);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        worldTransformLookup.Update(ref state);

        BeginSimulationEntityCommandBufferSystem.Singleton entityCommandBufferSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer entityCommandBuffer = entityCommandBufferSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        TurretShoot turretShootJob = new TurretShoot
        {
            WorldTransformLookup = worldTransformLookup,
            ECB = entityCommandBuffer
        };

        turretShootJob.Schedule();
    }
}

[BurstCompile]
partial struct TurretShoot : IJobEntity
{
    [ReadOnly] public ComponentLookup<WorldTransform> WorldTransformLookup;
    public EntityCommandBuffer ECB;

    void Execute(in TurretAspect turret)
    {
        Entity instance = ECB.Instantiate(turret.CannonBallPrefab);
        WorldTransform spawnLocalToWorld = WorldTransformLookup[turret.CannonBallSpawn];
        LocalTransform cannonBallTransform = LocalTransform.FromPosition(spawnLocalToWorld.Position);

        cannonBallTransform.Scale = WorldTransformLookup[turret.CannonBallPrefab].Scale;
        ECB.SetComponent(instance, cannonBallTransform);
        ECB.SetComponent(instance, new CannonBall
        {
            Speed = spawnLocalToWorld.Forward() * 20f
        });

        ECB.SetComponent(instance, new URPMaterialPropertyBaseColor { Value = turret.Color });
    }
}