using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct TurretRotationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        quaternion rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

        foreach (TransformAspect transform in SystemAPI.Query<TransformAspect>().WithAll<Turret>())
        {
            transform.RotateWorld(rotation);
        }
    }
}
