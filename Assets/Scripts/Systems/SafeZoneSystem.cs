using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[WithAll(typeof(Turret))]
[BurstCompile]
partial struct SafeZoneJob : IJobEntity
{
    [NativeDisableParallelForRestriction] public ComponentLookup<Shooting> ShootingLookup;
    public float SquaredRadius;

    void Execute(Entity entity, TransformAspect transform)
    {
        ShootingLookup.SetComponentEnabled(entity, math.lengthsq(transform.WorldPosition) > SquaredRadius);
    }
}

[BurstCompile]
partial struct SafeZoneSystem : ISystem
{
    ComponentLookup<Shooting> shootingLookup;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        shootingLookup = state.GetComponentLookup<Shooting>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float radius = SystemAPI.GetSingleton<Config>().SafeZoneRadius;
        const float debugRenderStepInDegrees = 20;

        for (float angle = 0; angle < 360; angle += debugRenderStepInDegrees)
        {
            float3 a = float3.zero;
            float3 b = float3.zero;
            math.sincos(math.radians(angle), out a.x, out a.z);
            math.sincos(math.radians(angle + debugRenderStepInDegrees), out b.x, out b.z);
            UnityEngine.Debug.DrawLine(a * radius, b * radius);
        }

        shootingLookup.Update(ref state);
        SafeZoneJob safeZoneJob = new SafeZoneJob
        {
            ShootingLookup = shootingLookup,
            SquaredRadius = radius * radius
        };
        safeZoneJob.ScheduleParallel();
    }
}